using Cloud.Caching;
using Cloud.Models;
using Cloud.Services;
using Cloud.Utilities;
using Cloud.Utilities.Json;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;
using Xg.Cloud.BankMallH5.Cloud;
using Xg.Cloud.BankMallH5.Cloud.Model;
using Xg.Cloud.BankMallH5.Cloud.QueryModel;

namespace Xg.Cloud.BankMallH5
{
    public class BankMallH5Service: IBankMallH5Service
    {
        /// <summary>
        /// 商户号
        /// </summary>
        private readonly string? mid = default;
        /// <summary>
        /// 终端号
        /// </summary>
        private readonly string? tid = default;
        /// <summary>
        /// 支付结果通知地址
        /// </summary>
        private readonly string? notifyUrl = default;
        /// <summary>
        /// 支付完跳转地址
        /// </summary>
        private readonly string? returnUrl = default;
        private readonly string? accessTokenKey = default;
        private readonly string? baseUrl = default;
        private readonly string? appId = default;
        private readonly string? appKey = default;
        private readonly string? md5Key = default;
        private readonly string? instalBankName=default;
        private readonly ILogger<BankMallH5Service> _logger;
        private readonly ICache _cache;
        private readonly IHttpClientService _httpClientService;

  

        public BankMallH5Service(ILogger<BankMallH5Service> logger, IConfiguration configuration, ICache cache, IHttpClientService httpClientService)
        {
            mid = configuration["RemoteServices:CloudBankMallH5:Mid"];
            tid = configuration["RemoteServices:CloudBankMallH5:Tid"];
            notifyUrl = configuration["RemoteServices:CloudBankMallH5:NotifyUrl"];
            returnUrl = configuration["RemoteServices:CloudBankMallH5:ReturnUrl"];
            accessTokenKey = configuration["RemoteServices:CloudBankMallH5:AccessTokenKey"];
            baseUrl = configuration["RemoteServices:CloudBankMallH5:BaseUrl"];
            appId = configuration["RemoteServices:CloudBankMallH5:AppId"];
            appKey = configuration["RemoteServices:CloudBankMallH5:AppKey"];
            md5Key = configuration["RemoteServices:CloudBankMallH5:Md5Key"];
            instalBankName = configuration["RemoteServices:CloudBankMallH5:InstalBankName"];

            _logger = logger;
            _cache = cache;
            _httpClientService = httpClientService;


        }
        public async Task<QrcodeModel> GetQrcode(QrcodeQueryModel qrcodeQueryModel)
        {
            qrcodeQueryModel.instMid = "QRINSTALDEFAULT";
            qrcodeQueryModel.mid = mid;
            qrcodeQueryModel.tid = tid;
            qrcodeQueryModel.notifyUrl = notifyUrl;
            qrcodeQueryModel.requestTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            qrcodeQueryModel.expireTime = DateTime.Now.AddMinutes(3).ToString("yyyyMMddHHmmss");
            qrcodeQueryModel.timeStart = DateTime.Now.ToString("yyyyMMddHHmmss");
            qrcodeQueryModel.instalBankNameList = instalBankName;
            qrcodeQueryModel.isForceLimitInstalNum = "01";
            qrcodeQueryModel.returnUrl = returnUrl;
            qrcodeQueryModel.unionLogin = true;

            var bodyData = JsonUtility.Serialize(qrcodeQueryModel);

            string requestUrl = $"{baseUrl}/v1/netpay/upfq/get-qrcode";
            try
            {
                var result =await _httpClientService.BankHttpPost(requestUrl, bodyData, await GetAccessToken());
                var res = JsonUtility.Deserialize<QrcodeModel>(result);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"银联接口get-qrcode 请求异常：{ex}");
                throw new MyException("get-qrcode error！");
            }

        }
        public async Task<OrderReturnModel> OrderReturn(OrderReturnQueryModel orderReturnQueryModel)
        {
            orderReturnQueryModel.instMid = "QRINSTALDEFAULT";
            orderReturnQueryModel.mid = mid;
            orderReturnQueryModel.tid = tid;
            orderReturnQueryModel.requestTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var bodyData = JsonUtility.Serialize(orderReturnQueryModel);

            string requestUrl = $"{baseUrl}/v1/netpay/upfq/refund";
            try
            {
                var result =await _httpClientService.BankHttpPost(requestUrl, bodyData, await GetAccessToken());
                var res = JsonUtility.Deserialize<OrderReturnModel>(result);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"银联接口refund 请求异常：{ex}");
                throw new MyException("refund error！");
            }

        }

        /// <summary>
        /// 订单关闭
        /// </summary>
        /// <param name="merOrderId">商户订单号 原交易单号</param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public async Task<OrderCloseModel> OrderColse(string merOrderId)
        {
            OrderCloseQueryModel orderCloseQueryModel = new OrderCloseQueryModel
            {
                instMid = "QRINSTALDEFAULT",
                mid = mid,
                tid = tid,
                requestTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                merOrderId = merOrderId
            };

            var bodyData = JsonUtility.Serialize(orderCloseQueryModel);

            string requestUrl = $"{baseUrl}/v1/netpay/close";
            try
            {
                var result = await _httpClientService.BankHttpPost(requestUrl, bodyData, await GetAccessToken());
                var res = JsonUtility.Deserialize<OrderCloseModel>(result);
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"银联接口close 请求异常：{ex}");
                throw new MyException("close error！");
            }

        }
        public async Task ReturnQuery(string merOrderId)
        {
            OrderCloseQueryModel orderCloseQueryModel = new OrderCloseQueryModel
            {
                instMid = "QRINSTALDEFAULT",
                mid = mid,
                tid = tid,
                requestTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                merOrderId = merOrderId,
                msgId="123456",
                srcReserve="Namedddd"
            };
           

            var bodyData = JsonUtility.Serialize(orderCloseQueryModel);
            
            string requestUrl = $"{baseUrl}/v1/netpay/upfq/refund-query";
    
            try
            {
                //var result =HttpUtility .BankHttpPost(requestUrl, bodyData, await GetAccessToken());
                var result = await _httpClientService.BankHttpPost(requestUrl, bodyData,await GetAccessToken());
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"银联接口refund 请求异常：{ex}");
                throw new MyException("refund error！");
            }
        }
        public async Task Test1()
        {
            var qrcodeQueryModel = new
            {
                msgId = "test",
                instMid = "H5DEFAULT",
                merOrderId = "123456",
                mid = mid,
                tid = tid,
                requestTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                srcReserve = "",
            };
            string bodyData = "{\"msgId\":\"0DUNOje1Bwtue6XinEs7DWhKS2NmUCVg\",\"requestTimestamp\":\"2024-05-13 13:37:18\",\"srcReserve\":\"保留字段\",\"merOrderId\":\"124S2023071980881897\",\"instMid\":\"QRINSTALDEFAULT\",\"mid\":\"898201612345678\",\"tid\":\"88880001\"}";

            string bodyData1 = JsonUtility.Serialize(qrcodeQueryModel);

            string requestUrl1 = $"{baseUrl}/v1/netpay/upfq/refund-query";
            string requestUrl = $"{baseUrl}/v1/netpay/refund-query";
            var result =await _httpClientService.BankHttpPost(requestUrl, bodyData, await GetAccessToken());
            var res = JsonUtility.Deserialize<QrcodeModel>(result);

        }
        public async Task<string?> GetAccessToken()
        {
            if (_cache.Exists(accessTokenKey))
            {
                return await _cache.GetAsync<string>(accessTokenKey);
            }
            AccessTokenQueryModel request = new AccessTokenQueryModel
            {
                AppId = appId,
                Nonce = new Random().Next(10000).ToString(),
                Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
                SignMethod= "SHA256"
            };
            try
            {
                request.Signature = GetSign(request);
                
                var bodyData = JsonUtility.Serialize(request);
                string requestUrl = $"{baseUrl}/v1/token/access?appId={request.AppId}&nonce={request.Nonce}&timestamp={request.Timestamp}&signature={request.Signature}";
                var res = JsonUtility.Deserialize<AccessTokenModel>(await _httpClientService.JsonPostAsync(requestUrl, bodyData));

                //var res1 = HttpUtility.HttpGet(requestUrl, bodyData);
                //var res= JsonUtility.Deserialize<AccessTokenModel>(res1);
                if (res.ErrCode == "0000")
                {
                    await _cache.SetFreeTimeAsync(accessTokenKey, res.AccessToken, new TimeSpan(0, 50, 0));
                    return res.AccessToken;
                }
                else
                {
                    throw new MyException($"获取accesstoken错误：{res}");
                }
            }
            catch (Exception ex)
            {
                throw new MyException($"获取accesstoken异常：{ex}");
            }
        }

        private string GetSign(AccessTokenQueryModel request)
        {
           
            var c = $"{request.AppId}{request.Timestamp}{request.Nonce}{appKey}";
            return EncryptionUtility.SHA2(c) ;
        }


    }
}
