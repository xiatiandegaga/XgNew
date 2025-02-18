using System;
using System.Threading.Tasks;

namespace Cloud.SMS
{
    public interface ISendVerifySms
    {
        void SendVerifySms(string phoneNumber, string code);
        void SendCouponSms(string phoneNumber, string[] msgArray);

        void SendSms(string phoneNumber, string templateId, string[] msgArray);

        void Send(Action<SmsOptions> setupAction);

        Task<T> SendAll<T>(Action<SmsOptions> setupAction) where T : class;
    }
}
