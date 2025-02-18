using COSXML;
using COSXML.Model.Object;
using Microsoft.Extensions.Options;

namespace Cloud.TencentCos
{
    public class CosUtility
    {
        private readonly CosConfigOptions _cosConfig;
        private readonly CosXml _cosXmlServer;
        private readonly string _imageExtension = ".png,.jpg,.jpeg,.gif";
        public CosUtility(IOptions<CosConfigOptions> cosConfigOptions, CosXml cosXmlServer)
        {
            _cosConfig = cosConfigOptions.Value;
            _cosXmlServer = cosXmlServer;
        }

        public void PutObject(byte[] fileData, string cosKey, string fileExtension)
        {
            PutObjectRequest request = new PutObjectRequest(_cosConfig.Bucket, cosKey, fileData);
            if (_imageExtension.Contains(fileExtension.ToLower()))
                request.SetRequestHeader("Content-Type", "image/jpeg");
            _cosXmlServer.PutObject(request);
        }

        public void PutVideo(byte[] fileData, string cosKey)
        {
            PutObjectRequest request = new PutObjectRequest(_cosConfig.Bucket, cosKey, fileData);
            request.SetRequestHeader("Content-Type", "video/mp4");
            _cosXmlServer.PutObject(request);
        }

        public byte[] DownloadObject(string cosKey)
        {
            GetObjectBytesRequest request = new GetObjectBytesRequest(_cosConfig.Bucket, cosKey);
            //执行请求
            GetObjectBytesResult result = _cosXmlServer.GetObject(request);
            //请求成功
            return result.content;
        }
    }
}
