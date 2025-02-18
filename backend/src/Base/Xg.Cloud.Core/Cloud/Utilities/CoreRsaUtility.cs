using RSAExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.Core.Cloud.Utilities
{
    public class CoreRsaUtility
    {
        /// <summary>
        /// 导入私钥
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="type"></param>
        /// <param name="privateKey"></param>
        /// <param name="isPem"></param>
        public static void ImportPrivateKey(RSA rsa, RSAKeyType type, string privateKey, bool isPem = false)
        {
            rsa.ImportPrivateKey(type,privateKey,isPem);
        }

        /// <summary>
        /// 导入公钥
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="type"></param>
        /// <param name="publicKey"></param>
        /// <param name="isPem"></param>
        public static void ImportPublicKey(RSA rsa, RSAKeyType type, string publicKey, bool isPem = false)
        {
            rsa.ImportPublicKey(type, publicKey, isPem);
        }

        /// <summary>
        /// 加密-使用公钥加密，私钥解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rsa"></param>
        /// <param name="encryptionPadding"></param>
        /// <returns></returns>
        public static string Encrypt(string data, RSA rsa, RSAEncryptionPadding encryptionPadding)
        {
            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(data), encryptionPadding));
        }

        /// <summary>
        /// 解密-使用公钥加密，私钥解密
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="rsa"></param>
        /// <param name="encryptionPadding"></param>
        /// <returns></returns>
        public static string Decrypt(byte[] sign, RSA rsa, RSAEncryptionPadding encryptionPadding)
        {
            return Convert.ToBase64String(rsa.Decrypt(sign, encryptionPadding));
        }

        /// <summary>
        /// 签名-使用私钥签名，公钥验签。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rsa"></param>
        /// <param name="hashName"></param>
        /// <param name="rsaPadding"></param>
        /// <returns></returns>
        public static byte[] SignData(string data,RSA rsa, HashAlgorithmName hashName, RSASignaturePadding rsaPadding)
        {
            return rsa.SignData(Encoding.UTF8.GetBytes(data), hashName, rsaPadding);
        }

        /// <summary>
        /// 验签-使用私钥签名，公钥验签。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sign"></param>
        /// <param name="rsa"></param>
        /// <param name="hashName"></param>
        /// <param name="rsaPadding"></param>
        /// <returns></returns>
        public static bool VerifyData(string data,byte[] sign, RSA rsa, HashAlgorithmName hashName, RSASignaturePadding rsaPadding)
        {
            return rsa.VerifyData(Encoding.UTF8.GetBytes(data), sign, hashName, rsaPadding);
        }
        /// <summary>
        /// 导出私钥
        /// </summary>
        /// <param name="rsa">rsa</param>
        /// <param name="type">类型</param>
        /// <param name="usePemFormat">usePemFormat</param>
        /// <returns></returns>
        public static string ExportPrivateKey( RSA rsa, RSAKeyType type, bool usePemFormat = false)
        {
            return rsa.ExportPrivateKey(type, usePemFormat);
        }
        /// <summary>
        ///  导出公钥
        /// </summary>
        /// <param name="rsa">rsa</param>
        /// <param name="type">类型</param>
        /// <param name="usePemFormat">usePemFormat</param>
        /// <returns></returns>
        public static string ExportPublicKey(RSA rsa, RSAKeyType type, bool usePemFormat = false)
        {
            return rsa.ExportPublicKey(type, usePemFormat);
        }
    }
}
