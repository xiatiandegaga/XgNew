using Cloud.Models;
using Cloud.Utilities.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get请求外部资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramer"></param>
        /// <returns></returns>
        public async Task<string> GetAjaxAsync(string url, Dictionary<object, object> paramer = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求

            #region 请求参数
            string urlParams = "";
            if (paramer != null && paramer.Count > 0)
            {
                foreach (var item in paramer.Keys)
                {
                    urlParams += item + "=" + paramer[item] + "&";
                }
                urlParams = urlParams.Trim('&');
                url = url + "?" + urlParams;
            }
            #endregion
            return await client.GetStringAsync(url);
        }


        /// <summary>
        /// 用于服务之间的Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramer"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url, Dictionary<object, object> paramer = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            #region 请求参数
            string urlParams = "";
            if (paramer != null && paramer.Count > 0)
            {
                foreach (var item in paramer.Keys)
                {
                    urlParams += item + "=" + paramer[item] + "&";
                }
                urlParams = urlParams.Trim('&');
                url = url + "?" + urlParams;
            }
            #endregion
            return await client.GetStringAsync(url);
        }

        /// <summary>
        /// 用于服务之间的Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramer"></param>
        /// <returns></returns>
        public async Task<string> GetStringParamerAsync(string url, Dictionary<string, string> paramer = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            #region 请求参数
            string urlParams = "";
            if (paramer != null && paramer.Count > 0)
            {
                foreach (var item in paramer.Keys)
                {
                    urlParams += item + "=" + paramer[item] + "&";
                }
                urlParams = urlParams.Trim('&');
                url = url + "?" + urlParams;
            }
            #endregion
            return await client.GetStringAsync(url);
        }

        public async Task<T> GetFromJsonAsync<T>(string url, Dictionary<object, object> paramer = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            #region 请求参数
            string urlParams = "";
            if (paramer != null && paramer.Count > 0)
            {
                foreach (var item in paramer.Keys)
                {
                    urlParams += item + "=" + paramer[item] + "&";
                }
                urlParams = urlParams.Trim('&');
                url = url + "?" + urlParams;
            }
            #endregion
            var result = JsonUtility.Deserialize<AjaxResponseGen<T>>(await client.GetStringAsync(url));
            if (result.Code == 0)
                return default;
            return result.Data;
        }

        /// <summary>
        /// 用户服务之间的post表单请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <returns></returns>
        public async Task<string> FormPostAsync(string url, Dictionary<string, string> formData = null)
        {
            var client = _httpClientFactory.CreateClient();
            FormUrlEncodedContent reContent = null;
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            if (formData != null)
                reContent = new FormUrlEncodedContent(formData);
            var response = await client.PostAsync(url, reContent);
            return await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 用于服务之间的post json请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> JsonPostAsync(string url, string content)
        {
            var client = _httpClientFactory.CreateClient();
            var reContent = new StringContent(content);
            reContent.Headers.Clear();
            reContent.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            var response = await client.PostAsync(url, reContent);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 用于服务之间的post json请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<byte[]> JsonPostByteAsync(string url, string content)
        {
            var client = _httpClientFactory.CreateClient();
            var reContent = new StringContent(content);
            reContent.Headers.Clear();
            reContent.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            var response = await client.PostAsync(url, reContent);
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<T> PostFromJsonAsync<T>(string url, string content)
        {
            var client = _httpClientFactory.CreateClient();
            var reContent = new StringContent(content);
            reContent.Headers.Clear();
            reContent.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            var response = await client.PostAsync(url, reContent);
            return JsonUtility.DeserializeByte<T>(await response.Content.ReadAsByteArrayAsync());
        }

        public async Task<string> DeleteAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");//模拟ajax请求
            var response = await client.DeleteAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="url">URL</param>        
        /// <param name="param">POST的数据</param>
        ///  <param name="file">file</param>
        /// <param name="fileByte">图片</param>
        /// <returns></returns>
        public string UploadFile(string url, IDictionary<object, object> param, string file, byte[] fileByte)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();
            string responseStr = null;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            if (param != null)
            {
                foreach (string key in param.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, param[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
            }

            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", file, "text/plain");//image/jpeg
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(fileByte, 0, fileByte.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseStr = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                throw new Exception(ex.Message);
            }
            return responseStr;
        }/// <summary>
         /// 用于有银联接口请求
         /// </summary>
         /// <param name="url"></param>
         /// <param name="content"></param>
         /// <param name="token"></param>
         /// <returns></returns>
        public string BankHttpPost1(string url, string content, string token)
        {
            WebClient webClient = new WebClient();
            Encoding encoding = Encoding.UTF8;

            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("Authorization", "OPEN-ACCESS-TOKEN AccessToken=" + token);
            byte[] postData = encoding.GetBytes(content);
            byte[] responseData = webClient.UploadData(url, "POST", postData);
            string responseContent = encoding.GetString(responseData);
            return responseContent;
        }
        /// <summary>
        /// 用于有银联接口请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> BankHttpPost(string url, string content,string token)
        {
            var client = _httpClientFactory.CreateClient("default");
            var reContent = new StringContent(content, Encoding.UTF8);
            reContent.Headers.Clear();
            reContent.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.DefaultRequestHeaders.Add("Authorization", $"OPEN-ACCESS-TOKEN AccessToken={token}");
      
            var response = await client.PostAsync(url, reContent);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
