using Cloud.Extensions;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Snowflake;
using Domain.Entity.Product;
using Domain.Interface;
using Domain.IService.Identity;
using Domain.IService.Product;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.Admin.Output;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.Service.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class MallProductService(ICloudUnitOfWork unitWork, IListCacheRepository<MallProduct> listCacheMallProductRepository, IListCacheRepository<MallProductAttr> listCacheMallProductAttrRepository, IListCacheRepository<MallProductSku> listCacheMallProductSkuRepository, IListCacheRepository<MallProductType> listCacheMallProductTypeRepository, IListCacheRepository<MallProductCategory> listCacheMallProductCategoryRepository, IMallProductSkuStockHistoryService mallProductSkuStockHistoryService, IMallProductSkuService mallProductSkuService, IAuthenticationPrincipalService authenticationPrincipalService, ISnowflakeIdWorker snowflakeIdWorker) : BaseListCacheService<MallProduct, MallProductDto>(listCacheMallProductRepository), IMallProductService
    {
        private readonly ICloudUnitOfWork _unitWork = unitWork;
        private readonly IListCacheRepository<MallProduct> _listCacheMallProductRepository = listCacheMallProductRepository;
        private readonly IListCacheRepository<MallProductAttr> _listCacheMallProductAttrRepository = listCacheMallProductAttrRepository;
        private readonly IListCacheRepository<MallProductSku> _listCacheMallProductSkuRepository = listCacheMallProductSkuRepository;
        private readonly IListCacheRepository<MallProductType> _listCacheMallProductTypeRepository = listCacheMallProductTypeRepository;
        private readonly IListCacheRepository<MallProductCategory> _listCacheMallProductCategoryRepository = listCacheMallProductCategoryRepository;
        private readonly IMallProductSkuStockHistoryService _mallProductSkuStockHistoryService = mallProductSkuStockHistoryService;
        private readonly IMallProductSkuService _mallProductSkuService = mallProductSkuService;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService = authenticationPrincipalService;
        private readonly ISnowflakeIdWorker _snowflakeIdWorker=snowflakeIdWorker;

        public override async Task<MallProductDto> GetSingleByIdAsync(IdQueryCommonInput input)
        {
            var mallProductDto =  _unitWork.Query<MallProduct>(x => x.Id == input.Id).ProjectToType<MallProductDto>().FirstOrDefault();
            if (mallProductDto == default)
            {
                return default;
            }
            await GetDtoAsync(mallProductDto);
            return mallProductDto;
        }

        public async Task<MallProductDto> FindSingleByIdForH5Async(IdQueryCommonInput input)
        {
            var mallProductDto = _unitWork.Query<MallProduct>(x => x.Id == input.Id).ProjectToType<MallProductDto>().FirstOrDefault();
            if (mallProductDto == default)
            {
                return default;
            }
            await GetDtoAsync(mallProductDto,CommonConst.Yes);
            return mallProductDto;
        }

        public override async Task AddOrUpdateAsync(MallProductDto productDto)
        {
            if (productDto == default)
            {
                throw new MyException("对象不能为空！", CommonConst.No);
            }

            var productEntity = productDto.MapTo<MallProduct>();
            var mallProductSkuDtos = productDto.MallProductSkuDtos;
            var mallProductAttrList = productDto.MallProductAttrDtos.MapToIEnumerable<MallProductAttrDto,MallProductAttr>().ToList();
          
            if (string.IsNullOrWhiteSpace(productDto.ProductName))
            {
                throw new MyException("商品名称不能为空！", CommonConst.No);
            }
            if (productDto.MallProductTypeId == default)
            {
                throw new MyException("商品类型不能为空！", CommonConst.No);
            }
            if (productDto.MallProductCategoryId == default)
            {
                throw new MyException("商品目录不能为空！", CommonConst.No);
            }

            if(mallProductSkuDtos==default||mallProductSkuDtos.Count==0)
            {
                throw new MyException("sku不能为空！", CommonConst.No);
            }

            if (productEntity.Id > 0)
            {
                if(await _unitWork.ExistsAsync<MallProduct>(x=>x.Id!=productEntity.Id&&x.MallProductTypeId==productEntity.MallProductTypeId&&x.MallProductCategoryId==productEntity.MallProductCategoryId&&x.ProductName==productEntity.ProductName))
                {
                    throw new MyException($"该目录下已存在名称为【{productEntity.ProductName}】的商品");
                }
                _unitWork.ExecuteDelete<MallProductAttr>(x=>x.MallProductId==productEntity.Id);

                var oldProductSkuList = _unitWork.Query<MallProductSku>(x => x.MallProductId == productEntity.Id).ToList();
                #region 编辑的时候不能删除sku
                oldProductSkuList.ForEach(item =>
                {
                    if (!mallProductSkuDtos.Any(x => x.Id == item.Id))
                    {
                        throw new MyException($"sku不能删除", CommonConst.No);
                    }
                });
                #endregion
                _unitWork.Update(productEntity);
            }
            else
            {
                if (await _unitWork.ExistsAsync<MallProduct>(x => x.MallProductTypeId == productEntity.MallProductTypeId && x.MallProductCategoryId == productEntity.MallProductCategoryId && x.ProductName == productEntity.ProductName))
                {
                    throw new MyException($"该目录下已存在名称为【{productEntity.ProductName}】的商品");
                }
                productEntity.Id= _snowflakeIdWorker.NextId();
                productEntity.Status = CommonConst.No;
                _unitWork.Add(productEntity);
            }

            if (mallProductAttrList != default && mallProductAttrList.Count > 0)
            {
                mallProductAttrList.ForEach(item =>
                {
                    item.Id = _snowflakeIdWorker.NextId();
                    item.MallProductId = productEntity.Id;
                });
                _unitWork.AddRange(mallProductAttrList);
            }
            var loginUser = await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            mallProductSkuDtos.ToList().ForEach(skuDto =>
            {
                skuDto.MallProductId = productEntity.Id;
                skuDto.SkuPrice = (skuDto.SkuPriceAmount * 100).ToInt();
                skuDto.SkuInnerPrice = (skuDto.SkuInnerPriceAmount * 100).ToInt();
                skuDto.SkuName = productEntity.ProductName;
                var skuEntity = skuDto.MapTo<MallProductSku>();
                if (skuEntity.Id > 0)
                {
                    _unitWork.Update(skuEntity);
                }
                else
                {
                    skuEntity.Id = _snowflakeIdWorker.NextId();
                    if(skuEntity.SkuStock>0)
                    {
                        _mallProductSkuStockHistoryService.SaveInfo(new Entity.MallProductSkuStockHistory { GoodsAttrs = skuEntity.AttrKeyValue, SkuId = skuEntity.Id, Num = skuEntity.SkuStock, StockType = CommonConst.StockType_1, StockDetailType = CommonConst.ProductInvOrRelType_StockInv,ProductId=skuEntity.MallProductId },_unitWork,loginUser.Id);
                    }
                    _unitWork.Add(skuEntity);
                }
            });
            await _unitWork.CommitAsync();
            Task.WaitAll(_unitWork.RemoveListCacheAsync<MallProduct>(), _unitWork.RemoveListCacheAsync<MallProductSku>(), _unitWork.RemoveListCacheAsync<MallProductAttr>()); 
        }
        public List<MallProduct> GetAllListByTypeId(long typeId)
        {
            var result = _listCacheMallProductRepository.Query(x=>x.MallProductTypeId==typeId).ToList();
            return result;
        }

        public async Task<List<MallProduct>> GetAllListAsync()
        {
            var result =await  _listCacheMallProductRepository.GetListAsync();
            return result;
        }
        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public async Task TakeOnAsync(long id)
        {
            var productEntity =await _unitWork.GetSingleAsync<MallProduct>(x=>x.Id
            ==id);
            if(productEntity == null)
            {
                throw new MyException("商品不存在！");
            }
            if(productEntity.Status==CommonConst.Yes)
            {
                throw new MyException("商品已上架，请勿重复操作！");
            }
            if(await _unitWork.ExistsAsync<MallProductSku>(x=>x.MallProductId==productEntity.Id&&x.Status==CommonConst.Yes&&x.SkuPrice<=0))
            {
                throw new MyException($"商品中存在未维护价格的sku！");
            }
            productEntity.Status = CommonConst.Yes;
            _unitWork.Update(productEntity);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProduct>();
        }
        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public async Task TakeOffAsync(long id)
        {
            var productEntity = await _unitWork.GetSingleAsync<MallProduct>(x => x.Id
            == id);
            if (productEntity == null)
            {
                throw new MyException("商品不存在！");
            }
            if (productEntity.Status == CommonConst.No)
            {
                throw new MyException("商品已下架，请勿重复操作！");
            }

            productEntity.Status = CommonConst.No;
            _unitWork.Update(productEntity);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProduct>();
        }
        public async Task BatchTakeOnAsync(List<long> listId)
        {
            if (listId == default || listId.Count == 0) throw new MyException("请选择要上架的商品！", CommonConst.No);
            var list = _unitWork.Query<MallProduct>(x => listId.Contains(x.Id)).ToList();
            var skuList =await  _listCacheMallProductSkuRepository.GetListAsync();
            if (list != default && list.Count > 0)
            {
                list.ForEach(item => {
                    if(skuList.Any(x=>x.MallProductId==item.Id&&x.Status==CommonConst.Yes&&x.SkuPrice<=0))
                    {
                        throw new MyException($"商品【{item.ProductName}】中存在未维护价格的sku！");
                    }

                    item.Status = CommonConst.Yes;
                });
            }
            _unitWork.UpdateRange(list);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProduct>();
        }

        public async Task BatchTakeOffAsync(List<long> listId)
        {
            if (listId == default || listId.Count == 0) throw new MyException("请选择要下架的商品！", CommonConst.No);
            var list = _unitWork.Query<MallProduct>(x => listId.Contains(x.Id)).ToList();
            if (list != default && list.Count > 0)
            {
                list.ForEach(item => {
                    item.Status = CommonConst.No;
                });
            }
            _unitWork.UpdateRange(list);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProduct>();
        }

        /// <summary>
        /// 通过类型编号获取所有上架商品并分类分组
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public async Task<dynamic> GetProductsGroupbyCategoryByTypeCode(string typeCode)
        {
            var mallProductType =await _listCacheMallProductTypeRepository.GetSingleAsync(x => x.TypeCode == typeCode);
            if (mallProductType == null)
                return null;

            var listDto = (await _listCacheMallProductRepository.QueryAsync(x => x.MallProductTypeId == mallProductType.Id && x.Status == CommonConst.Yes, x => x.SortNo)).MapToList<MallProduct,MallProductDto>();

            var goodsCategoryList = _listCacheMallProductCategoryRepository.Query(x => x.MallProductTypeId == mallProductType.Id,true,x=>x.SortNo).ToList();

            if (listDto != null && listDto.Count > 0)
            {
                listDto.ForEach(async dto =>
                {
                    await GetDtoAsync(dto,CommonConst.Yes);
                });
            }

            return goodsCategoryList.Where(x => x.Pid == 0).GroupBy(x => x.Id, (key, group) => new AdminMallProductsGroupOutput { Category = group.FirstOrDefault().MapTo<MallProductCategoryDto>(),  ProductDtos = listDto.Where(x => x.MallProductCategoryId == key).OrderBy(x=>x.SortNo).ToList() });
        }


        public async Task GetDtoAsync(MallProductDto dto,int? status=default)
        {
            var mallProductSkuDtos = (await _listCacheMallProductSkuRepository.QueryAsync(x => x.MallProductId == dto.Id)).OrderBy(x => x.SortNo).MapToList<MallProductSku,MallProductSkuDto>();
            if (mallProductSkuDtos != default && mallProductSkuDtos.Count > 0)
            {
                if (status != default)
                {
                    mallProductSkuDtos = mallProductSkuDtos.Where(x => x.Status == status).ToList();
                }
                mallProductSkuDtos.ForEach(item =>
                {
                    item.SkuPriceAmount = ((item.SkuPrice * 1.00) / 100).ToDecimal();
                    item.SkuInnerPriceAmount = ((item.SkuInnerPrice * 1.00) / 100).ToDecimal();

                    item.NumberOfInstallmentModelList = GetNumberOfInstallmentModels(item.SkuPriceAmount, item.NumberOfInstallments);

                    item.InnerNumberOfInstallmentModelList = GetNumberOfInstallmentModels(item.SkuInnerPriceAmount, item.InnerNumberOfInstallments);
                });
                var effectSkus = mallProductSkuDtos.Where(x => x.Status == CommonConst.Yes).ToList();
                if (effectSkus != default && effectSkus.Count > 0) {
                    dto.MaxPriceAmount = effectSkus.Max(x => x.SkuPriceAmount);

                    dto.InnerMaxPriceAmount = effectSkus.Max(x => x.SkuInnerPriceAmount);

                    var minPriceSkuItem = effectSkus.OrderBy(x => x.SkuPriceAmount).ThenBy(x => x.SortNo).FirstOrDefault();

                    dto.MinPriceAmount = minPriceSkuItem.SkuPriceAmount;
                    dto.NumberOfInstallmentModel = minPriceSkuItem.NumberOfInstallmentModelList.OrderByDescending(x => x.NumberOfInstallments).FirstOrDefault();

                    var innerMinPriceSkuItem = effectSkus.OrderBy(x => x.SkuInnerPriceAmount).ThenBy(x => x.SortNo).FirstOrDefault();
                    dto.InnerMinPriceAmount = innerMinPriceSkuItem.SkuInnerPriceAmount;

                    dto.InnerNumberOfInstallmentModel = innerMinPriceSkuItem.InnerNumberOfInstallmentModelList.OrderByDescending(x => x.NumberOfInstallments).FirstOrDefault();
                }
            }

            var mallProductAttrDtos =(await _listCacheMallProductAttrRepository.QueryAsync(x => x.MallProductId == dto.Id)).MapToList<MallProductAttr,MallProductAttrDto>();
            dto.MallProductSkuDtos = mallProductSkuDtos;
            dto.MallProductAttrDtos = mallProductAttrDtos;
        }

        private List<NumberOfInstallmentOutput> GetNumberOfInstallmentModels(decimal price,string numberOfInstallment)
        {
            List<NumberOfInstallmentOutput> numberOfInstallmentModelList = new List<NumberOfInstallmentOutput>();
            if(string.IsNullOrWhiteSpace(numberOfInstallment)) return numberOfInstallmentModelList;
            string[] numberOfInstallmentList= numberOfInstallment.Split(',',StringSplitOptions.RemoveEmptyEntries);
            if(numberOfInstallmentList==default|| numberOfInstallmentList.Length==0)
            {
                numberOfInstallmentModelList.Add(new NumberOfInstallmentOutput() {  Price=price, NumberOfInstallments=0});
            }

            foreach(var item in numberOfInstallmentList)
            {
                NumberOfInstallmentOutput model = new NumberOfInstallmentOutput();
                model.NumberOfInstallments = item.ToInt();
                if(model.NumberOfInstallments == 0) {
                    model.Price = price;
                }
                else
                {
                    //向上取整并保留2位小数
                    model.Price =  ((Math.Ceiling((price/ model.NumberOfInstallments)*100))/100).ToDecimal(2);
                }
                numberOfInstallmentModelList.Add((model));

            }

            return numberOfInstallmentModelList;
        }
        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public async Task StockInv(AdminMallSkuStockChangeInput input)
        {
            var sku =await _unitWork.GetSingleAsync<MallProductSku>(x=>x.Id==input.SkuId);
            if(sku==default)
            {
                throw new MyException("sku不存在！",CommonConst.No);
            }

            if(input.Num<=0)
            {
                throw new MyException("入库数量必须大于0！", CommonConst.No);
            }

            List<AdminMallSkuStockChangeInfoInput> list = new List<AdminMallSkuStockChangeInfoInput>()
            {
                new AdminMallSkuStockChangeInfoInput()
                {
                     ChangeFreezeStockCount=0,
                     ChangeSkuStockCount=input.Num,
                     StockType=CommonConst.StockType_1,
                     StockDetailType=CommonConst.ProductInvOrRelType_StockInv,
                     SkuId=sku.Id,
                }
            };
            var loginUser =await  _authenticationPrincipalService.GetAuthenticatedUserAsync();
            _mallProductSkuService.SaveStock(list,_unitWork, loginUser.Id);

            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProductSku>();
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public async Task StockRel(AdminMallSkuStockChangeInput input)
        {
            var sku =await _unitWork.GetSingleAsync<MallProductSku>(x => x.Id == input.SkuId);
            if (sku == default)
            {
                throw new MyException("sku不存在！", CommonConst.No);
            }

            if (input.Num <= 0)
            {
                throw new MyException("出库数量必须大于0！", CommonConst.No);
            }

            List<AdminMallSkuStockChangeInfoInput> list = new List<AdminMallSkuStockChangeInfoInput>()
            {
                new AdminMallSkuStockChangeInfoInput()
                {
                     ChangeFreezeStockCount=0,
                     ChangeSkuStockCount=input.Num,
                     StockType=CommonConst.StockType_2,
                     StockDetailType=CommonConst.ProductInvOrRelType_StockRel,
                     SkuId=sku.Id,
                }
            };
            var loginUser =await  _authenticationPrincipalService.GetAuthenticatedUserAsync();
            _mallProductSkuService.SaveStock(list, _unitWork, loginUser.Id);

            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProductSku>();
        }
    }
}
