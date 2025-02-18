using Cloud.Models.MiniProgramModel;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.IService.Identity
{
    public interface IWechatLoginService : ICloudService
    {
        Task<JsCode2SessionResponse> WeChatLogin(string code);
        Task<string> GetWeChatToken();
        Task<string> ReGetWeChatToken();
        /// <summary>
        /// 获取永久小程序二维码
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="type">类型（1门店推广活动 2机器码）</param>
        /// <param name="width">宽度 默认430</param>
        Task GetPermanentCode(string id, int type, int width = 430);
        /// <summary>
        /// 生成微信小程序码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task GetUnlimited(long id);
    }
}
