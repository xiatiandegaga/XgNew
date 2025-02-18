using Cloud.Extensions;
using Cloud.Models;
using Cloud.Utilities;
using Cloud.Utilities.Json;
using Microsoft.Extensions.Configuration;
using WebApiClientCore;
using WebApiClientCore.HttpContents;

namespace Cloud.Parking.KeTuo.Model
{
    public class ParkingPaymentInfoRequest: BaseAuthRequest, IApiParameter
    {

        /// <summary>
        /// 接口业务代码
        /// </summary>
        public string serviceCode { get; set; } = "getParkingPaymentInfo";

        /// <summary>
        /// 车牌号
        /// </summary>
        public string plateNo { get; set; }

        /// <summary>
        /// 免费时长（秒）
        /// </summary>
        public int freeTime { get; set; } = 0;

        /// <summary>
        /// 免费金额（单位:分）
        /// </summary>
        public int freeMoney { get; set; } = 0;

        public Task OnRequestAsync(ApiParameterContext context)
        {
            var config = (IConfiguration)context.HttpContext.ServiceProvider.GetService(typeof(IConfiguration));
            if (config == default)
                throw new MyException($"{nameof(ParkingPaymentInfoRequest)} 不可为空",0);
            var appSercert = config["Parking:KeTuo:AppSecret"];
            var req = new ParkingPaymentInfoRequest
            {
                serviceCode=this.serviceCode,
                plateNo =this.plateNo,
                appId = config["Parking:KeTuo:AppId"],
                parkId = Convert.ToInt32(config["Parking:KeTuo:ParkId"]),
                ts = TimeExtension.CurrentTimeMillis().ToString(),
                reqId= Guid.NewGuid().ToString("N")
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
