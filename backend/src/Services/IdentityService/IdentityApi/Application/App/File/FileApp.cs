using Cloud.Mvc;
using Cloud.Mvc.Filters;
using Cloud.TencentCos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.File
{
    public class FileApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly CosUtility _cosUtility;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileApp(CosUtility cosUtility)
        {
            _cosUtility = cosUtility;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> UploadFileAsync(IFormCollection form)
        {
            var lstShowPaths = new List<string>();
            foreach (var file in form.Files)
            {
                var guid = Guid.NewGuid().ToString("N");
                var fileExtension = Path.GetExtension(file.FileName);
                var cosKey = guid + fileExtension;
                lstShowPaths.Add(cosKey);
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                _cosUtility.PutObject(fileBytes, cosKey, fileExtension);

            }
            return lstShowPaths;
        }

        /// <summary>
        /// 视频上传
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<string>> PutVideoAsync(IFormCollection form)
        {
            var lstShowPaths = new List<string>();
            foreach (var file in form.Files)
            {
                var guid = Guid.NewGuid().ToString("N");
                var fileExtension = Path.GetExtension(file.FileName);
                var cosKey = guid + fileExtension;
                lstShowPaths.Add(cosKey);
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                _cosUtility.PutVideo(fileBytes, cosKey);

            }
            return lstShowPaths;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="downName"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, NoWrapper]
        public FileContentResult DownloadFile(string fileName, string downName = null)
        {
            byte[] buffer = _cosUtility.DownloadObject(fileName);
            FileContentResult fileContent = new(buffer, "application/octet-stream")
            {
                FileDownloadName = downName ?? fileName
            };
            return fileContent;
        }

    }
}
