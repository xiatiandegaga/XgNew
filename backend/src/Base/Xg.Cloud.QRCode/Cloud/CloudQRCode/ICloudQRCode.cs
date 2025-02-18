using System.Drawing;

namespace Cloud.CloudQRCode
{
    public interface ICloudQRCode
    {
        Bitmap GetQRCode(string plainText, int pixel);
    }
}
