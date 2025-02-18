using Cloud.Models;
using Cloud.Mvc;
using Domain.Interface;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App
{
    public class MallProductSkuStockHistoryApp : ICloudApp, ICloudDynamicWebApi
    {

        private readonly IMallProductSkuStockHistoryService  _mallProductSkuStockHistoryService;
        public MallProductSkuStockHistoryApp(IMallProductSkuStockHistoryService mallProductSkuStockHistoryService)
        {
            _mallProductSkuStockHistoryService = mallProductSkuStockHistoryService;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<MallProductSkuStockHistoryDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallProductSkuStockHistoryService.GetPageListAsync(input);
            return result;
        }
    }
}
