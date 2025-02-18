using Cloud.Mvc;
using Domain.IService.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App
{
    public class AutoApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallOrderService _orderService;
        public AutoApp(IMallOrderService orderService) {
            _orderService = orderService;
        }

        /// <summary>
        /// 自动签收发货7天的单据
        /// </summary>
        [HttpPost,AllowAnonymous]
        
        public async Task AutoSignFor()
        {
            await _orderService.AutoSignFor();
        }
    }
}
