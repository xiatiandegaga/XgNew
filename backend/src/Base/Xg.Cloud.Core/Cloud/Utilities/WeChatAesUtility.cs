using System;
using System.Security.Cryptography;
using System.Text;

namespace Cloud.Utilities
{
    public class WeChatAesUtility
    {
        #region 解密
        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="aesIV">向量128</param>  
        /// <param name="aesKey">key</param>  
        /// <param name="inputdata">输入的数据encryptedData</param>  
        /// <returns name="result">解密后的字符串</returns>  
        public static string AESDecrypt(string aesIV, string aesKey, string inputdata)
        {
            try
            {
                aesIV = aesIV.Replace(" ", "+");
                aesKey = aesKey.Replace(" ", "+");
                inputdata = inputdata.Replace(" ", "+");
                byte[] encryptedData = Convert.FromBase64String(inputdata);

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(aesKey); // Encoding.UTF8.GetBytes(AesKey);  
                rijndaelCipher.IV = Convert.FromBase64String(aesIV);// Encoding.UTF8.GetBytes(AesIV);  
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);
                return result;
            }
            catch (Exception)
            {
                return null;

            }
        }
        #endregion
    }
}
