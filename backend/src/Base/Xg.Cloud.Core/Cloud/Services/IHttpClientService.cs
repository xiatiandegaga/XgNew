using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloud.Services
{
    public interface IHttpClientService
    {
        Task<string> GetAjaxAsync(string url, Dictionary<object, object> paramer = null);
        Task<string> GetAsync(string url, Dictionary<object, object> paramer = null);

        Task<string> GetStringParamerAsync(string url, Dictionary<string, string> paramer = null);

        Task<T> GetFromJsonAsync<T>(string url, Dictionary<object, object> paramer = null);

        Task<string> FormPostAsync(string url, Dictionary<string, string> formData = null);

        Task<string> JsonPostAsync(string url, string content);

        Task<byte[]> JsonPostByteAsync(string url, string content);

        Task<T> PostFromJsonAsync<T>(string url, string content);

        Task<string> DeleteAsync(string url);

        string UploadFile(string url, IDictionary<object, object> param, string file, byte[] fileByte);
        ///<summary>
        /// 用于有银联接口请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        string BankHttpPost1(string url, string content, string token);
        /// <summary>
        /// 用于有银联接口请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> BankHttpPost(string url, string content, string token);
    }
}
