using Cloud.Caching;
using Cloud.Models.HttpClientUtility;
using Cloud.Models.MiniProgramModel;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Services;
using Cloud.TencentCos;
using Cloud.Utilities.Json;
using Domain.IService.Identity;
using Identity.Shared.Dto.WeChat.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp.Formats;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service.Identity
{
    public class WechatLoginService : IWechatLoginService
    {
        private readonly IWeChatLoginClient _weChatLoginClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientService _httpClienService;
        private readonly ICache _cache;
        private readonly IAuthenticationPrincipalService _authenticationService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CosUtility _cosUtility;
        private readonly ICloudUnitOfWork _unitWork;

        public WechatLoginService(IWeChatLoginClient weChatLoginClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientService httpClienService, ICache cache, IAuthenticationPrincipalService authenticationService, IHttpClientFactory httpClientFactory, CosUtility cosUtility, ICloudUnitOfWork unitWork)
        {
            _weChatLoginClient = weChatLoginClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClienService = httpClienService;
            _cache = cache;
            _authenticationService = authenticationService;
            _httpClientFactory = httpClientFactory;
            _cosUtility = cosUtility;
            _unitWork = unitWork;
        }

        public async Task<JsCode2SessionResponse> WeChatLogin(string code)
        {
            var response = await _weChatLoginClient.GetData(_configuration["MiniProgram:Api:jscode2session"].Replace("<code>", code));
            return JsonUtility.Deserialize<JsCode2SessionResponse>(response);
        }


        /// <summary>
        /// 微信小程序token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWeChatToken()
        {
            // 将验证码的key放入cookie
            string tokenKey = _configuration["MiniProgram:Api:tokenKey"];
            if (!string.IsNullOrWhiteSpace(tokenKey))
            {
                if (await _cache.ExistsAsync(tokenKey)) return await _cache.GetAsync<string>(tokenKey);
                return await ReGetWeChatToken();
            }
            else
            {
                return await GetAccessToken();
            }
        }
        /// <summary>
        /// 重新请求微信接口获取token,并重置缓存tokenKey
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReGetWeChatToken()
        {
            string tokenKey = _configuration["MiniProgram:Api:tokenKey"];
            string token = await GetAccessToken();
            await _cache.SetAsync(tokenKey, token, CachingExpireType.SingleObject, false);
            return token;
        }
        /// <summary>
        /// 请求微信接口获取token
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAccessToken()
        {
            var response = await _weChatLoginClient.GetData(_configuration["MiniProgram:Api:weChatToken"]);
            return JsonUtility.Deserialize<WeChatTokenInput>(response).access_token;
        }

        /// <summary>
        /// 获取永久小程序二维码
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="type">类型（1门店推广活动 2机器码）</param>
        /// <param name="width">宽度 默认430</param>
        public async Task GetPermanentCode(string key, int type, int width = 430)
        {
            string token = await GetWeChatToken();
            var jsonData = JsonUtility.Serialize(new { scene = $"key={key}&t={type}", page = "pages/new_preload/home", width });
            var content = new StringContent(jsonData, Encoding.UTF8);
            var result = await _httpClientFactory.CreateClient().PostAsync(_configuration["MiniProgram:Api:wxacode"] + $"?access_token={token}", content);
            byte[] buffer = await result.Content.ReadAsByteArrayAsync();
            if (buffer.Length < 5000)
            {
                token = await ReGetWeChatToken();
                buffer = await _httpClienService.JsonPostByteAsync(_configuration["MiniProgram:Api:wxacode"] + $"?access_token={token}", jsonData);
            }
            var response = _httpContextAccessor.HttpContext.Response;
            response.ContentType = "image/jpeg";
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 生成微信小程序码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task GetUnlimited(long id)
        {
            string token = await GetWeChatToken();
            var jsonData = JsonUtility.Serialize(new { scene = $"pid={id}", page = "pages/new_preload/home", width = 430 });
            var content = new StringContent(jsonData, Encoding.UTF8);
            var result = await _httpClientFactory.CreateClient().PostAsync(_configuration["MiniProgram:Api:wxacode"] + $"?access_token={token}", content);
            byte[] buffer = await result.Content.ReadAsByteArrayAsync();
            if (buffer.Length < 5000)
            {
                token = await ReGetWeChatToken();
                buffer = await _httpClienService.JsonPostByteAsync(_configuration["MiniProgram:Api:wxacode"] + $"?access_token={token}", jsonData);
            }
            var response = _httpContextAccessor.HttpContext.Response;
            response.ContentType = "image/jpeg";
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 合并图片 小图片放在大图片上面
        /// </summary>
        /// <param name="templebytes">二维码图片</param>
        /// <param name="outputbytes">模板图片</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">y坐标</param>
        /// <returns></returns>
        public async Task MergeImage(byte[] templeBytes, byte[] imageBytes)
        {
            string strRet = null;
            IImageFormat format = null;
            //var imagesTemle = SixLabors.ImageSharp.Image.Load(templeBytes, out format);
            //var outputImg = SixLabors.ImageSharp.Image.Load(imageBytes);
            var imagesTemle = new Bitmap(new MemoryStream(templeBytes));
            var outputImg = new Bitmap(new MemoryStream(imageBytes));
            decimal ratio = 1;//根据宽度等比缩放
            int minWidth = imagesTemle.Width;
            if (outputImg.Width < minWidth)
            {
                minWidth = outputImg.Width;
                ratio = imagesTemle.Width / minWidth;
                imagesTemle = KiResizeImage(imagesTemle, minWidth, Convert.ToInt32(imagesTemle.Height / ratio));
            }
            else
            {
                ratio = outputImg.Width * 1.0000m / minWidth;
                outputImg = KiResizeImage(outputImg, minWidth, Convert.ToInt32(outputImg.Height / ratio));
                //outputImg.Mutate(ctx => ctx.Resize(minWidth, Convert.ToInt32(outputImg.Height / ratio)));
            }

            // 上下留白高度(设置10px,可自行修改)
            int c = 1,
             // 画布宽度
             width = minWidth + 2 * c,
             // 画布高度
             height = 0,
             // 数组元素个数(即要拼图的图片个数)
             lenth = 2;

            // 图片集合
            Bitmap[] maps = new Bitmap[lenth];
            // 图片对应纵坐标集合
            int[] pointY = new int[lenth + 1];
            // 第一张图y轴起始未知
            pointY[0] = c;
            // 记录画布的总高度
            height = pointY[0];

            maps[0] = new Bitmap(outputImg);
            maps[1] = new Bitmap(imagesTemle);
            for (int i = 0; i < lenth; i++)
            {
                // 计算并保存照片拼接时Y轴起始位置
                pointY[i + 1] = maps[i].Height + height + c;
                // 记录画布总高度
                height = pointY[i + 1];
            }

            // 初始化画布(最终的拼图画布)并设置宽高
            Bitmap bitMap = new Bitmap(width, height);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            // 将画布涂为白色(底部颜色可自行设置)
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));

            // 图片的Bitmap集合
            for (int m = 0; m < maps.Length; m++)
            {
                // 画布宽度
                for (int i = 0; i < maps[m].Width; i++)
                {
                    // 画布高度
                    for (int j = 0; j < maps[m].Height; j++)
                    {
                        // 以像素点形式绘制(将要拼图的图片上的每个坐标点绘制到拼图对象的指定位置，直至所有点都绘制完成)
                        var temp = maps[m].GetPixel(i, j);
                        // 将图片画布的点绘制到整体画布的指定位置
                        bitMap.SetPixel(c + i, pointY[m] + j, temp);
                    }
                }
                maps[m].Dispose();
            }
            // 保存输出到本地
            var ms = new MemoryStream();
            bitMap.Save(ms, ImageFormat.Jpeg);

            g1.Dispose();
            bitMap.Dispose();
            byte[] buffer = ms.GetBuffer();
            var response = _httpContextAccessor.HttpContext.Response;
            response.ContentType = "image/jpeg";
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
        }


        ///
        /// Resize图片
        ///
        /// 原始Bitmap
        /// 新的宽度
        /// 新的高度
        /// 保留着，暂时未用
        /// 处理以后的图片
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
