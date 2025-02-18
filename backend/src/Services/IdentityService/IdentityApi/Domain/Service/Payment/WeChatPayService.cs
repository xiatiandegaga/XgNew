using Cloud.Utilities.Json;
using Domain.IService.Payment;
using Essensoft.Paylink.WeChatPay;
using Essensoft.Paylink.WeChatPay.V3;
using Essensoft.Paylink.WeChatPay.V3.Domain;
using Essensoft.Paylink.WeChatPay.V3.Request;
using Essensoft.Paylink.WeChatPay.V3.Response;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Domain.Service.Payment
{
    public class WeChatPayService : IWeChatPayService
    {
        private readonly IWeChatPayClient _weChatPayClient;
        private readonly IOptions<WeChatPayOptions> _optionsAccessor;
        public WeChatPayService(IWeChatPayClient weChatPayClient,IOptions<WeChatPayOptions> optionsAccessor)
        {
            _weChatPayClient=weChatPayClient;
            _optionsAccessor = optionsAccessor;
        }
        public async Task<string> WeChatPay(WeChatPayTransactionsJsApiBodyModel input)
        {
            var model = new WeChatPayTransactionsJsApiBodyModel
            {
                //AppId = _optionsAccessor.Value.AppId,
                //MchId = _optionsAccessor.Value.MchId,
                //Amount = new Amount { Total = viewModel.Total, Currency = "CNY" },
                //Description = viewModel.Description,
                //NotifyUrl = viewModel.NotifyUrl,
                //OutTradeNo = viewModel.OutTradeNo,
                //Payer = new PayerInfo { OpenId = viewModel.OpenId }
            };
            var request = new WeChatPayTransactionsJsApiRequest();
            request.SetBodyModel(input);

            var response = await _weChatPayClient.ExecuteAsync(request, _optionsAccessor.Value);

            if (!response.IsError)
            {
                var req = new WeChatPayMiniProgramSdkRequest
                {
                    Package = "prepay_id=" + response.PrepayId
                };

                var parameter = await _weChatPayClient.ExecuteAsync(req, _optionsAccessor.Value);
                // 将参数(parameter)给 小程序端
                // https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter3_5_4.shtml
                return JsonUtility.Serialize(parameter);
            }
            return null;
        }

        public async Task<WeChatPayTransactionsIdResponse> OrderQuery(WeChatPayTransactionsIdRequest input)
        {
            var model = new WeChatPayTransactionsIdQueryModel
            {
                MchId = _optionsAccessor.Value.MchId,
            };

            var request = new WeChatPayTransactionsIdRequest
            {
                TransactionId = input.TransactionId
            };
            request.SetQueryModel(model);
            var response =  await _weChatPayClient.ExecuteAsync(request, _optionsAccessor.Value);
            return response;
        }

        public async Task<WeChatPayRefundDomesticRefundsResponse> Refund(WeChatPayRefundDomesticRefundsBodyModel input)
        {
            var model = new WeChatPayRefundDomesticRefundsBodyModel()
            {
                TransactionId = input.TransactionId,
                OutTradeNo = input.OutTradeNo,
                OutRefundNo = input.OutRefundNo,
                NotifyUrl = input.NotifyUrl,
                //Amount = new RefundAmount { Refund = viewModel.RefundAmount, Total = viewModel.TotalAmount, Currency = viewModel.Currency }
            };

            var request = new WeChatPayRefundDomesticRefundsRequest();
            request.SetBodyModel(model);

            var response = await _weChatPayClient.ExecuteAsync(request, _optionsAccessor.Value);
            return response;
        }

        public async Task<WeChatPayRefundDomesticRefundsOutRefundNoResponse> RefundQuery(WeChatPayRefundDomesticRefundsOutRefundNoRequest input)
        {
            var request = new WeChatPayRefundDomesticRefundsOutRefundNoRequest
            {
                OutRefundNo = input.OutRefundNo
            };

            var response = await _weChatPayClient.ExecuteAsync(request, _optionsAccessor.Value);
            return response;
        }
    }
}
