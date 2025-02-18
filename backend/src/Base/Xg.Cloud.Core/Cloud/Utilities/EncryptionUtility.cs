using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cloud.Utilities
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public static class EncryptionUtility
    {

        #region 对称/非对称

        /// <summary>
        /// 对称加密
        /// </summary>
        /// <param name="encryptType">加密类型</param>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="ivString">初始化向量</param>
        /// <param name="keyString">加密密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string SymmetricEncrypt(SymmetricEncryptType encryptType, string str, string ivString, string keyString)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(ivString) || string.IsNullOrEmpty(keyString))
                return str;
            SymmetricEncrypt encrypt = new SymmetricEncrypt(encryptType);
            encrypt.IVString = ivString;
            encrypt.KeyString = keyString;
            return encrypt.Encrypt(str);
        }

        /// <summary>
        /// 对称解密
        /// </summary>
        /// <param name="encryptType">加密类型</param>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="ivString">初始化向量</param>
        /// <param name="keyString">加密密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string SymmetricDncrypt(SymmetricEncryptType encryptType, string str, string ivString, string keyString)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            SymmetricEncrypt encrypt = new SymmetricEncrypt(encryptType);
            encrypt.IVString = ivString;
            encrypt.KeyString = keyString;
            return encrypt.Decrypt(str);
        }

        #endregion

        #region MD5

        /// <summary>
        /// 标准MD5加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// 新版本，性能强
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5_HexConvert(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);
            return Convert.ToHexString(hashBytes);
        }


        public static string MD5Up(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("X").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// 16位的MD5加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5_16(string str)
        {
            return MD5(str).Substring(8, 16);
        }

        #endregion

        #region base64编码/解码

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">待编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string Base64_Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="str">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string Base64_Decode(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        #endregion

        /// <summary>
        /// 使用sha1加密字符串。
        /// </summary>
        /// <param name="inputString">输入字符串。</param>
        /// <returns>加密后的字符串。（40个字符）</returns>
        public static string SHA1(string inputString)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] encryptedBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 使用sha2加密字符串。
        /// </summary>
        /// <returns>加密后的字符串。（40个字符）</returns>
        public static string SHA2(string inputString, string format = "{0:X2}")
        {
            SHA256CryptoServiceProvider sha2 = new SHA256CryptoServiceProvider();
            byte[] encryptedBytes = sha2.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat(format, encryptedBytes[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        ///  使用sha2加密字符串。（64个字符）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="_input_charset"></param>
        /// <returns></returns>
        public static string GetSHA256hash(string input)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(input);
            SHA256 sha256 = new SHA256Managed();
            sha256.ComputeHash(clearBytes);
            byte[] hashedBytes = sha256.Hash;
            sha256.Clear();
            string output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return output;
        }


        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string hexStr)
        {
            hexStr = hexStr.Replace(" ", "");
            if ((hexStr.Length % 2) != 0)
                hexStr += " ";
            byte[] returnBytes = new byte[hexStr.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="baSrc"></param> 
        /// <returns></returns> 
        public static string ByteToHexStr(byte[] baSrc)
        {
            if (baSrc == null)
            {
                return "";
            }

            int nByteNum = baSrc.Length;
            StringBuilder sbResult = new StringBuilder(nByteNum * 2);

            for (int i = 0; i < nByteNum; i++)
            {
                char chHex;

                byte btHigh = (byte)((baSrc[i] & 0xF0) >> 4);
                if (btHigh < 10)
                {
                    chHex = (char)('0' + btHigh);
                }
                else
                {
                    chHex = (char)('A' + (btHigh - 10));
                }
                sbResult.Append(chHex);

                byte btLow = (byte)(baSrc[i] & 0x0F);
                if (btLow < 10)
                {
                    chHex = (char)('0' + btLow);
                }
                else
                {
                    chHex = (char)('A' + (btLow - 10));
                }
                sbResult.Append(chHex);
            }
            return sbResult.ToString();
        }

        /// <summary>
        /// AES加密（无偏移量）
        /// </summary>
        /// <param name="sKey">密钥</param>
        /// <param name="sStr">加密参数</param>
        /// <returns></returns>
        public static string AESEncryption(string sKey, string sStr)
        {
            string sResult = string.Empty;
            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.Mode = CipherMode.ECB;
            oAes.Padding = PaddingMode.PKCS7;
            oAes.Key = Convert.FromBase64String(sKey);
            byte[] byteSource = Encoding.UTF8.GetBytes(sStr);
            byte[] byteTransfer = oAes.CreateEncryptor().TransformFinalBlock(byteSource, 0, byteSource.Length);

            sResult = ByteToHexStr(byteTransfer);
            return sResult;
        }

        /// <summary>
        /// AES解密（无偏移量）
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sSource"></param>
        /// <returns></returns>
        public static string AESDecrypt(string sKey, string sSource)
        {
            byte[] encryptedBytes = HexStringToBytes(sSource);

            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(sKey.PadRight(bKey.Length)), bKey, bKey.Length);
            MemoryStream mStream = new MemoryStream(encryptedBytes);
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(sKey);
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="sKey">密钥</param>
        /// <param name="sIV">密钥</param>
        /// <param name="sStr">加密参数</param>
        /// <returns></returns>
        public static string AESEncryption(string sKey, string sIV, string sStr)
        {
            string sResult = string.Empty;
            int ivLength = 32;
            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.Mode = CipherMode.ECB;
            oAes.Padding = PaddingMode.PKCS7;
            oAes.Key = Convert.FromBase64String(sKey);
            oAes.IV = new byte[ivLength];
            oAes.IV = Encoding.Default.GetBytes(sIV);
            byte[] byteSource = Encoding.UTF8.GetBytes(sStr);
            byte[] byteTransfer = oAes.CreateEncryptor().TransformFinalBlock(byteSource, 0, byteSource.Length);

            sResult = ByteToHexStr(byteTransfer);
            return sResult;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sIV"></param>
        /// <param name="sSource"></param>
        /// <returns></returns>
        public static string AESDecrypt(string sKey, string sIV, string sSource)
        {
            byte[] encryptedBytes = HexStringToBytes(sSource);
            //Byte[] sKeyBytes = ;
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(sKey.PadRight(bKey.Length)), bKey, bKey.Length);
            MemoryStream mStream = new MemoryStream(encryptedBytes);
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(sKey);
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        /// AES加密二进制
        /// </summary>
        /// <param name="sKey">密钥</param>
        /// <param name="byteSource">加密参数</param>
        /// <returns></returns>
        public static byte[] AESEncryption(string sKey, byte[] byteSource)
        {
            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.Mode = CipherMode.ECB;
            oAes.Padding = PaddingMode.PKCS7;
            oAes.Key = Convert.FromBase64String(sKey);
            byte[] byteTransfer = oAes.CreateEncryptor().TransformFinalBlock(byteSource, 0, byteSource.Length);
            return byteTransfer;
        }

        /// <summary>
        /// AES解密二进制
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="byteSourcee"></param>
        /// <returns></returns>
        public static string AESDecrypt(string sKey, byte[] byteSourcee)
        {
            byte[] encryptedBytes = byteSourcee;

            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(sKey.PadRight(bKey.Length)), bKey, bKey.Length);
            MemoryStream mStream = new MemoryStream(encryptedBytes);
            RijndaelManaged aes = new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
                Key = Convert.FromBase64String(sKey)
            };
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        public static string CalculateHMACSHA256(string message, string secretKey)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLowerInvariant();
            }
        }
        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="source">目标字符串</param>
        /// <returns></returns>
        public static string HMACSHA1(string secret, string source)
        {
            byte[] bsource = Encoding.UTF8.GetBytes(source);
            var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            var resultBytes = hmacsha1.ComputeHash(bsource);
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="inputdata">输入的数据encryptedData</param>  
        /// <param name="aesIV">key</param>  
        /// <param name="aesKey">向量128</param>  
        /// <returns name="result">解密后的字符串</returns>  
        public static string WeChatAESDecrypt(string aesIV, string aesKey, string inputdata)
        {
            try
            {
                aesIV = aesIV.Replace(" ", "+");
                aesKey = aesKey.Replace(" ", "+");
                inputdata = inputdata.Replace(" ", "+");
                byte[] encryptedData = Convert.FromBase64String(inputdata);

                RijndaelManaged rijndaelCipher = new RijndaelManaged
                {
                    Key = Convert.FromBase64String(aesKey), // Encoding.UTF8.GetBytes(AesKey);  
                    IV = Convert.FromBase64String(aesIV),// Encoding.UTF8.GetBytes(AesIV);  
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
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
    }
}
