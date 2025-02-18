using Essensoft.Paylink.WeChatPay.V3.Domain;
using Essensoft.Paylink.WeChatPay.V3.Request;
using Essensoft.Paylink.WeChatPay.V3.Response;
using System.Threading.Tasks;

namespace Domain.IService.Payment
{
    public interface IWeChatPayService
    {
        Task<string> WeChatPay(WeChatPayTransactionsJsApiBodyModel input);

        Task<WeChatPayTransactionsIdResponse> OrderQuery(WeChatPayTransactionsIdRequest input);

        Task<WeChatPayRefundDomesticRefundsResponse> Refund(WeChatPayRefundDomesticRefundsBodyModel input);

        Task<WeChatPayRefundDomesticRefundsOutRefundNoResponse> RefundQuery(WeChatPayRefundDomesticRefundsOutRefundNoRequest input);
    }
}
