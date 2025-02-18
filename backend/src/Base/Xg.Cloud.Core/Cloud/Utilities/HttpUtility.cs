using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Cloud.Utilities
{
    //[Obsolete("即将弃用，请使用IHttpClientService注入来替代")]
    public class HttpUtility
    {
        /// <summary>
        /// HTTP POST向服务器发送数据并获取结果
        /// </summary>
        /// <param name="Url">POST数据的服务器url地址</param>
        /// <param name="postDataStr">POST的数据参数</param>
        /// <returns>HTTP POST获取的结果</returns>
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        /// <summary>
        /// HTTP POST向服务器发送数据并获取结果
        /// </summary>
        /// <param name="url">POST数据的服务器url地址</param>
        /// <param name="postDataStr">POST的数据参数</param>
        /// <returns>HTTP POST获取的结果</returns>
        public static string HttpPostJson(string url, string postDataStr)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 30000;//设置请求超时时间，单位为毫秒
            req.ContentType = "application/json";
            byte[] data = Encoding.UTF8.GetBytes(postDataStr);
            req.ContentLength = data.Length;
      
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();

            //获取响应内容
            string result = "";
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;

        }

        /// <summary>
        /// 银联post请求接口
        /// </summary>
        /// <param name="url">POST数据的服务器url地址</param>
        /// <param name="postDataStr">POST的数据参数</param>
        /// <param name="token">token</param>
        /// <returns>HTTP POST获取的结果</returns>
        public static string BankHttpPost(string url, string postDataStr, string token=default)
        {
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 30000;//设置请求超时时间，单位为毫秒
            req.ContentType = "application/json";
            byte[] data = Encoding.UTF8.GetBytes(postDataStr);
            req.ContentLength = data.Length;
            req.Headers.Add("Authorization", $"OPEN-ACCESS-TOKEN AccessToken={token}");
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            Stream stream = resp.GetResponseStream();

            //获取响应内容
            string result = "";
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;

        }
        /// <summary>
        /// HTTP GET 从服务器获取数据
        /// </summary>
        /// <param name="Url">GET数据的服务器url地址</param>
        /// <param name="postDataStr">GET的数据参数</param>
        /// <returns>HTTP GET获取的结果</returns>
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }


        /// <summary>  
        /// 下载文件上传到指定地址  
        /// </summary>  
        /// <param name="getUrl">文件下载地址</param>  
        /// <param name="postUrl">文件上传地址</param>  
        /// <param name="fileName">file控件名称</param>  
        /// <param name="fileType">上传文件类型</param>  
        /// <param name="inputDic">其他表单元素</param>  
        /// <param name="fileStream">其他表单元素</param>  
        /// fileStream
        public static string PostFile(string getUrl, string postUrl, string fileName, string fileType, Dictionary<string, string> inputDic, Stream fileStream)
        {
            //CredentialCache cache = new CredentialCache();//创建缓存容器  
            //cache.Add(new Uri("http://www.westfruit.com/"), "Basic", new NetworkCredential("westfruit", "7766517"));
            //CookieContainer cookies = new CookieContainer();//创建cookie容器
            //MemoryStream fileStream = GetFileStream(getUrl);//下载文件返回内存流
            // cast the WebRequest to a HttpWebRequest since we're using HTTPS   
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");//元素分割标记  
            HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(postUrl);
            //httpWebRequest2.Credentials = cache;
            //httpWebRequest2.CookieContainer = cookies;
            httpWebRequest2.ContentType = "multipart/form-data; boundary=" + boundary;//其他地方的boundary要比这里多--  
            httpWebRequest2.Method = "POST";//Post请求方式  
            // Build up the post message 拼接创建表单内容   
            StringBuilder sb = new StringBuilder();
            //拼接非文件表单控件  
            //遍历字典取出表单普通空间的健和值  
            foreach (KeyValuePair<string, string> dicItem in inputDic)
            {
                sb.Append("--" + boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"" + dicItem.Key + "\"");
                sb.Append("\r\n");
                sb.Append("\r\n");
                sb.Append(dicItem.Value);//value前面必须有2个换行  
                sb.Append("\r\n");
            }
            //拼接文件控件  
            sb.Append("--" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"file1\"; filename=\"" + Path.GetFileName(fileName) + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: " + fileType);
            sb.Append("\r\n");
            sb.Append("\r\n");//value前面必须有2个换行  
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());
            // Build the trailing boundary string as a byte array 创建结束标记  
            // ensuring the boundary appears on a line by itself   
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            //http请求总长度  
            httpWebRequest2.ContentLength = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            Stream requestStream = httpWebRequest2.GetRequestStream(); //定义一个http请求流  
            // Write out our post header 将开始标记写入流  
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            // Write out the file contents 将附件写入流,最大4M  
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);
            fileStream.Dispose();
            // Write out the trailing boundary 将结束标记写入流  
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            //Send http request back WebResponse 发送http请求  
            var response = httpWebRequest2.GetResponse();

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        #region 解析html代码  
        /// <summary>  
        /// 从html流中解析出html代码  
        /// </summary>  
        /// <param name="htmlStream">html流</param>  
        /// <param name="Encoding">编码格式</param>  
        /// <returns>html</returns>  
        public static string GetHtml(Stream htmlStream, string Encoding)
        {
            StreamReader objReader = new StreamReader(htmlStream, System.Text.Encoding.GetEncoding(Encoding));
            string HTML = "";
            string sLine = "";
            int i = 0;
            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    HTML += sLine;
            }
            HTML = HTML.Replace("<", "<");
            HTML = HTML.Replace(">", ">");
            objReader.Dispose();
            return HTML;
        }
        #endregion
    }
}
