using System.Threading.Tasks;

namespace Cloud.Models.HttpClientUtility
{
    public interface IWeChatLoginClient
    {
        Task<string> GetData(string request);
    }
}
