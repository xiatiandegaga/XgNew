using Cloud.Extensions;
using Cloud.Models;
using Cloud.Utilities;
using Cloud.Utilities.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.HttpContents;

namespace Cloud.Parking.KeTuo.Model
{
    public class GetOrderStatusRequest : BaseAuthRequest, IApiParameter
    {
        /// <summary>
        /// 接口业务代码
        /// </summary>
        public string serviceCode { get; set; } = "getOrderStatus";

        /// <summary>
        /// 车场账单号，不允许为空
        /// </summary>
        public string orderNo { get; set; }


        public Task OnRequestAsync(ApiParameterContext context)
        {
            var config = (IConfiguration)context.HttpContext.ServiceProvider.GetService(typeof(IConfiguration));
            if (config == default)
                throw new MyException($"{nameof(ParkingPaymentInfoRequest)} 不可为空",0);
            var appSercert = config["Parking:KeTuo:AppSecret"];
            var req = new PayParkingFeeRequest
            {
                serviceCode = this.serviceCode,
                appId = config["Parking:KeTuo:AppId"],
                parkId =Convert.ToInt32(config["Parking:KeTuo:ParkId"]),
                ts = TimeExtension.CurrentTimeMillis().ToString(),
                reqId = Guid.NewGuid().ToString("N"),
                orderNo = this.orderNo
            };
            var waitSign = $"{ApiSignUtility.GetSortParam(JsonUtility.Serialize(req), "appId")}&{appSercert}";
            req.key = EncryptionUtility.MD5(waitSign).ToUpper();
            context.HttpContext.RequestMessage.Headers.Add("version", config["Parking:KeTuo:Version"]);
            var options = context.HttpContext.HttpApiOptions.JsonSerializeOptions;
            context.HttpContext.RequestMessage.Content = new JsonContent(req, options);
            return Task.CompletedTask;
        }
    }
}
