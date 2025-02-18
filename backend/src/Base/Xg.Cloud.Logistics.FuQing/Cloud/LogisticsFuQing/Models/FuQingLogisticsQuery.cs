using Cloud.Utilities.Json;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.LogisticsFuQing.Models
{
    public class FuQingLogisticsQuery : ILogisticsQuery
    {
        private readonly LogisticsConfigOptions _reqConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        public FuQingLogisticsQuery(IOptions<LogisticsConfigOptions> reqConfig, IHttpClientFactory httpClientFactory)
        {
            _reqConfig = reqConfig.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T> NoQuery<T>(LogisticsRequest request) where T : class
        {
            request.Url = _reqConfig.Url;
            request.Auth = _reqConfig.Auth;
            Dictionary<string, string> paramer = new Dictionary<string, string>()
            {
                {"type",request.Type },{"no",request.No }
            };
            var result = await GetAjaxAsync(request.Url, paramer, request.Auth);
            return JsonUtility.Deserialize<T>(result);
        }

        public async Task<T> GenericNoQuery<T>(LogisticsRequest request) where T : class
        {
            request.Url = _reqConfig.Url;
            request.Auth = _reqConfig.Auth;
            Dictionary<string, string> paramer = new Dictionary<string, string>()
            {
                {"type",request.Type },{"no",request.No }
            };
            var result = await GetAjaxAsync(request.Url, paramer, request.Auth);
            return JsonUtility.Deserialize<T>(result);
        }

        public async Task<FuQingNoQueryResponse> NoQuery(LogisticsRequest request)
        {
            request.Url = _reqConfig.Url;
            request.Auth = _reqConfig.Auth;
            Dictionary<string, string> paramer = new Dictionary<string, string>()
            {
                {"type",request.Type },{"no",request.No }
            };
            var result = await GetAjaxAsync(request.Url, paramer, request.Auth);
            var entity = JsonUtility.Deserialize<FuQingNoQueryResponse>(result);
            if (entity.Result != default && entity.Result.Deliverystatus != default)
                entity.Result.DeliverystatusName = LogisticsListDic.DicFuQingNoQueryDeliveryStatus[entity.Result.Deliverystatus];
            return entity;

        }

        public async Task<FuQingNoQueryResultResponse> NoQueryResult(LogisticsRequest request)
        {
            request.Url = _reqConfig.Url;
            request.Auth = _reqConfig.Auth;
            Dictionary<string, string> paramer = new Dictionary<string, string>()
            {
                {"type",request.Type },{"no",request.No }
            };
            var result = await GetAjaxAsync(request.Url, paramer, request.Auth);
            var entity = JsonUtility.Deserialize<FuQingNoQueryResponse>(result);
            if (entity.Result != default && entity.Result.Deliverystatus != default)
                entity.Result.DeliverystatusName = LogisticsListDic.DicFuQingNoQueryDeliveryStatus[entity.Result.Deliverystatus];
            return entity.Result;

        }

        public async Task<string> GetAjaxAsync(string url, Dictionary<string, string> paramer = default, string auth = default)
        {
            var client = _httpClientFactory.CreateClient();
            #region 请求参数
            string urlParams = "";
            if (paramer != default && paramer.Count > 0)
            {
                foreach (var item in paramer.Keys)
                {
                    urlParams += item + "=" + paramer[item] + "&";
                }
                urlParams = urlParams.Trim('&');
                url = url + "?" + urlParams;
            }
            if (auth != default)
                client.DefaultRequestHeaders.Add("Authorization", $"APPCODE {auth}");
            #endregion
            var responseMessage = await client.GetAsync(url);
            using var streamReader = new StreamReader(responseMessage.Content.ReadAsStreamAsync().Result, Encoding.GetEncoding("utf-8"));
            return streamReader.ReadToEnd();
        }
    }
}
