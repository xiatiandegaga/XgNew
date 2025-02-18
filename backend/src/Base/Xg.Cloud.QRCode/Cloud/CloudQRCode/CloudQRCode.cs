using QRCoder;
using System.Drawing;

namespace Cloud.CloudQRCode
{
    /// <summary>
    /// 生成二维码
    /// </summary>
    public class CloudQRCode : ICloudQRCode
    {
        public CloudQRCode()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public Bitmap GetQRCode(string plainText, int pixel)
        {
            var generator = new QRCodeGenerator();
            var qrCodeData = generator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
            var qrCodeIns = new QRCode(qrCodeData);
            var bitmap = qrCodeIns.GetGraphic(pixel, Color.Black, Color.White, true);
            return bitmap;
        }
    }
}
