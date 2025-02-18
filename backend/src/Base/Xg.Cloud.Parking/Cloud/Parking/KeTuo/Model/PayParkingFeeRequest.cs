using Cloud.Extensions;
using Cloud.Models;
using Cloud.Utilities;
using Cloud.Utilities.Json;
using Microsoft.Extensions.Configuration;
using WebApiClientCore;
using WebApiClientCore.HttpContents;

namespace Cloud.Parking.KeTuo.Model
{
    public class PayParkingFeeRequest : BaseAuthRequest, IApiParameter
    {
        /// <summary>
        /// 接口业务代码
        /// </summary>
        public string serviceCode { get; set; } = "payParkingFee";

        /// <summary>
        /// 车场账单号，不允许为空
        /// </summary>
        public string orderNo { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public int payableAmount { get; set; }

        /// <summary>
        /// 缴费时间, 'yyyy-mm-dd hh:mi:ss'
        /// </summary>
        public DateTime payTime { get; set; }

        /// <summary>
        /// 支付金额(不包含减免部分),单位为分，不允许为空
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// 收费终端，详见参数枚举1.6.4（4：微信平台 5：APP（安卓/IOS） 1000：其他）
        /// </summary>
        public int payType { get; set; }

        /// <summary>
        /// 付款方式，详见参数枚举 1.6.3针对amount的支付方式(1:现金 2:银行卡 3:电子现金 4:微信 5:支付宝6:城市E通卡7:其他 1013:积分抵扣)
        /// </summary>
        public int payMethod { get; set; }

        /// <summary>
        /// 减免总金额（单位 分）
        /// </summary>
        public int freeMoney { get; set; }

        /// <summary>
        /// 减免总时长（单位 秒）
        /// </summary>
        public int freeTime { get; set; }

        /// <summary>
        /// 车场是否离线(6.x系统无感支付及后付费用必须传.6.x支持、5.x不支持)：1、表示车辆已经离场
        /// </summary>
        public int isCarLeave { get; set; }

        /// <summary>
        /// json数组字符串（如果freeMoney或freeTime大于零时， 不能为空）
        /// </summary>
        public string freeDetail { get; set; }

        /// <summary>
        /// 商户订单号 非必填,主要用与后期本地数据和支付平台直接对账使用.
        /// </summary>
        public string outOrderNo { get; set; }

        /// <summary>
        /// 支付扩展信息，json对象
        /// </summary>
        public string paymentExt { get; set; }

        /// <summary>
        /// 是否无感支付，5.x无感支付时，需要传此参数，且值必须为1
        /// </summary>
        public int isNoSense { get; set; } = 0;


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
                parkId = Convert.ToInt32(config["Parking:KeTuo:ParkId"]),
                ts = TimeExtension.CurrentTimeMillis().ToString(),
                reqId = Guid.NewGuid().ToString("N"),
                payTime = DateTime.Now,
                orderNo = this.orderNo,
                payableAmount=this.payableAmount,
                amount=this.amount,
                payType=this.payType,
                payMethod=this.payMethod,
                freeMoney   =this.freeMoney,
                freeTime=this.freeTime,
                freeDetail=this.freeDetail,
                outOrderNo=this.outOrderNo,
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
