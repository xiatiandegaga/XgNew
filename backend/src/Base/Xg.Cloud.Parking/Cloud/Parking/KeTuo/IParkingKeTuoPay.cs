using Cloud.Parking.KeTuo.Model;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Cloud.Parking.KeTuo
{
    public interface IParkingKeTuoPay: IKeTuoApiService
    {
        [LoggingFilter]
        [HttpPost("/unite-api/api/wec/GetParkingPaymentInfo")]
        Task<BaseResponse<string>> GetParkingPaymentInfoAsync([JsonContent] ParkingPaymentInfoRequest request);

        [LoggingFilter]
        [HttpPost("/unite-api/api/wec/PayParkingFee")]
        Task<BaseResponse<string>> PayParkingFeeAsync([JsonContent] PayParkingFeeRequest request);

        [LoggingFilter]
        [HttpPost("/unite-api/api/wec/GetOrderStatus")]
        Task<BaseResponse<string>> GetOrderStatusAsync([JsonContent] GetOrderStatusRequest request);
        
    }
}
