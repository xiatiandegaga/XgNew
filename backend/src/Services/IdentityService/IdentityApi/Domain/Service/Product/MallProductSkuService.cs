using Cloud.Caching;
using Cloud.Mapster;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Utilities;
using Cloud.Utilities.Json;
using Domain.Entity.Product;
using Domain.Interface;
using Domain.IService.Identity;
using Domain.IService.Product;
using Domain.Service.Base;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;
using static FreeRedis.RedisClient;

namespace Domain.Service.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class MallProductSkuService : BaseService<MallProductSku, MallProductSkuDto>, IMallProductSkuService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly ICache _cache;
        private readonly ILogger<MallProductSkuService> _logger;
        private readonly IMallProductSkuStockHistoryService _mallProductSkuStockHistoryService;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService;

        public MallProductSkuService(IRepository<MallProductSku> repository, ICloudUnitOfWork unitWork, ICache cache, ILogger<MallProductSkuService> logger, IMallProductSkuStockHistoryService mallProductSkuStockHistoryService, IAuthenticationPrincipalService authenticationPrincipalService) : base(repository)
        {
            _unitWork = unitWork;
            _cache = cache;
            _logger = logger;
            _mallProductSkuStockHistoryService = mallProductSkuStockHistoryService;
            _authenticationPrincipalService= authenticationPrincipalService;
        }

        public async Task AddOrUpdateAsync(List<MallProductSkuDto> input)
        {
            var mallProductSkuList = input.MapToList<MallProductSkuDto, MallProductSku>();
            foreach (var x in mallProductSkuList)
            {
                //前端可能传来-1、-2这种
                if (x.Id > 0)
                {
                    _unitWork.Update(x);
                }
                else
                {
                    _unitWork.Add(x);
                }
            }
        }

        public void ChangeStock(List<AdminMallSkuStockChangeInfoInput> list, ICloudUnitOfWork uk,long userId)
        {
            #region 加锁部分
            var lockKey = ReflectionUtility.GetCurrentMethodFullName("lockKey");
            var timeoutSeconds = 20;
            LockController cacheLock = default;
            try
            {

                cacheLock = _cache.Lock(lockKey, timeoutSeconds);
                if (cacheLock != default)
                {
                    SaveStock(list, uk, userId);
                    cacheLock.Unlock();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"操作sku库存失败ChangeStock：{JsonUtility.Serialize(ex)}");
            }
            finally
            {
                cacheLock?.Unlock();
                cacheLock?.Dispose();
            }
            #endregion

        }

        public void SaveStock(List<AdminMallSkuStockChangeInfoInput> list, ICloudUnitOfWork uk,long userId)
        {
            if(list!=default&&list.Count>0)
            {
                var skuList=uk.Query<MallProductSku>(x=>list.Select(y=>y.SkuId).Contains(x.Id)).ToList();
                list.ForEach(item =>
                {
                    var sku = skuList.FirstOrDefault(x=>x.Id==item.SkuId);
                    if(sku!=default)
                    {
                        //表示冻结库存  并未实际出库
                        if(item.StockType==0)
                        {
                            sku.FreezeStock += item.ChangeFreezeStockCount;
                            sku.SkuStock-=item.ChangeSkuStockCount;
                        }
                        //表示入库
                        else if (item.StockType == CommonConst.StockType_1)
                        {
                            sku.SkuStock += item.ChangeSkuStockCount;
                            sku.FreezeStock -= item.ChangeFreezeStockCount;
                            _mallProductSkuStockHistoryService.SaveInfo(new Entity.MallProductSkuStockHistory { GoodsAttrs = sku.AttrKeyValue, SkuId = sku.Id, Num = item.ChangeSkuStockCount, StockType = item.StockType, StockDetailType = item.StockDetailType, ProductId = sku.MallProductId }, uk, userId);
                        }
                        //表示出库
                        else if (item.StockType == CommonConst.StockType_2)
                        {
                            int relCount = 0;
                            if(item.StockDetailType==CommonConst.ProductInvOrRelType_StockRel)
                            {
                                sku.SkuStock -= item.ChangeSkuStockCount;
                                relCount= item.ChangeSkuStockCount;
                            }
                            else if (item.StockDetailType == CommonConst.ProductInvOrRelType_OrderSaleRel)
                            {
                                sku.FreezeStock -= item.ChangeFreezeStockCount;
                                relCount = item.ChangeFreezeStockCount;
                            }

                            _mallProductSkuStockHistoryService.SaveInfo(new Entity.MallProductSkuStockHistory { GoodsAttrs = sku.AttrKeyValue, SkuId = sku.Id, Num = relCount, StockType = item.StockType, StockDetailType = item.StockDetailType, ProductId = sku.MallProductId }, uk,userId);
                        }
                    }

                    
                });

                uk.UpdateRange(skuList);
               
            }
        }

    }
}
