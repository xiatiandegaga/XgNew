using System.Net.Http;
using System.Threading.Tasks;

namespace Cloud.Models.HttpClientUtility
{
    public class WeChatLoginClient : IWeChatLoginClient
    {
        private readonly HttpClient _client;

        public WeChatLoginClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<string> GetData(string request)
        {
            return await _client.GetStringAsync(request);
        }
    }
}
