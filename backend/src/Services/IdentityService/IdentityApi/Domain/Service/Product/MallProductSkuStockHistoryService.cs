using Cloud.Extensions;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Snowflake;
using Domain.Entity;
using Domain.Entity.Dictionary;
using Domain.Entity.Product;
using Domain.Interface;
using Domain.IService.Identity;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.Service.Product
{
    public class MallProductSkuStockHistoryService(ICloudUnitOfWork unitWork, IRepository<MallProductSkuStockHistory> mallProductSkuStockHistoryRepository, IAuthenticationPrincipalService authenticationPrincipalService, IListCacheRepository<GlobalDataDetail> listCacheGlobalDataDetailRepository, IListCacheRepository<MallProduct> listCacheMallProductRepository, ISnowflakeIdWorker snowflakeIdWorker) : BaseService<MallProductSkuStockHistory, MallProductSkuStockHistoryDto>(mallProductSkuStockHistoryRepository), IMallProductSkuStockHistoryService
    {
        private readonly ICloudUnitOfWork _unitWork = unitWork;
        private readonly IRepository<MallProductSkuStockHistory> _mallProductSkuStockHistoryRepository = mallProductSkuStockHistoryRepository;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService = authenticationPrincipalService;
        private readonly IListCacheRepository<GlobalDataDetail> _listCacheGlobalDataDetailRepository = listCacheGlobalDataDetailRepository;
        private readonly IListCacheRepository<MallProduct> _listCacheMallProductRepository = listCacheMallProductRepository;
        private readonly ISnowflakeIdWorker _snowflakeIdWorker=snowflakeIdWorker;


        #region  数据查询
        public new async Task<PagingData<IEnumerable<MallProductSkuStockHistoryDto>>> GetPageListAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<MallProductSkuStockHistory>();

            var list = _mallProductSkuStockHistoryRepository.Paginate(input.PageNumber, input.PageSize, express, null, x => x.Id).ProjectToType<MallProductSkuStockHistoryDto>().ToList();
            GetListDto(list);
            var totalCount = await _mallProductSkuStockHistoryRepository.CountAsync(express);
            return new PagingData<IEnumerable<MallProductSkuStockHistoryDto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }


        private async Task GetListDto(List<MallProductSkuStockHistoryDto> listDto)
        {
            if (listDto != default)
            {
                var productList = await _listCacheMallProductRepository.GetListAsync();
                var globalDataDetailList = (await _listCacheGlobalDataDetailRepository.QueryAsync(x => x.Code == CommonConst.ProductInvOrRelType)).ToList();
                listDto.ForEach(dto =>
                {
                    GetDto(dto, globalDataDetailList, productList);
                });
            }
        }

        private void GetDto(MallProductSkuStockHistoryDto dto, List<GlobalDataDetail> globalDataDetailList,List<MallProduct> productList)
        {
            if (dto != default)
            {

                dto.MallProductName = productList.FirstOrDefault(x => x.Id == dto.ProductId)?.ProductName;

                if (!string.IsNullOrWhiteSpace(dto.StockDetailType))
                {
                    dto.StockDetailTypeName = globalDataDetailList.FirstOrDefault(x => x.ConstKey == dto.StockDetailType)?.Name;
                }
            }
        }
        #endregion

        public void SaveInfo(MallProductSkuStockHistory entity, ICloudUnitOfWork uw, long userId)
        {
            var id = _snowflakeIdWorker.NextId();
            string prex = entity.StockType == CommonConst.StockType_1? "INV":"REL";
            entity.Id = id;
            entity.StockNo = $"{prex}{id}";
            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = userId;
            uw.Add(entity);
        }
    }
}
