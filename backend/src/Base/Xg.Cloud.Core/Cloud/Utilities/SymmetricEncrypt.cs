using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cloud.Utilities
{
    /// <summary>
    /// 对称加密算法
    /// </summary>
    public class SymmetricEncrypt
    {
        private SymmetricEncryptType _mbytEncryptionType;
        private string _mstrOriginalString;
        private string _mstrEncryptedString;
        private SymmetricAlgorithm _mCSP;

        #region "Constructors"

        /// <summary>
        /// 默认采用DES算法
        /// </summary>
        public SymmetricEncrypt()
        {
            _mbytEncryptionType = SymmetricEncryptType.DES;
            this.SetEncryptor();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptionType">加密类型</param>
        public SymmetricEncrypt(SymmetricEncryptType encryptionType)
        {
            _mbytEncryptionType = encryptionType;
            this.SetEncryptor();
        }

        /// <summary>
        /// 构造行数
        /// </summary>
        /// <param name="encryptionType">加密类型</param>
        /// <param name="originalString">原始字符串</param>
        public SymmetricEncrypt(SymmetricEncryptType encryptionType, string originalString)
        {
            _mbytEncryptionType = encryptionType;
            _mstrOriginalString = originalString;
            this.SetEncryptor();
        }

        #endregion

        #region "Public Properties"

        /// <summary>
        /// 加密类型
        /// </summary>
        public SymmetricEncryptType EncryptionType
        {
            get { return _mbytEncryptionType; }
            set
            {
                if (_mbytEncryptionType != value)
                {
                    _mbytEncryptionType = value;
                    _mstrOriginalString = String.Empty;
                    _mstrEncryptedString = String.Empty;

                    this.SetEncryptor();
                }
            }
        }

        /// <summary>
        /// 对称加密算法提供者
        /// </summary>
        public SymmetricAlgorithm CryptoProvider
        {
            get { return _mCSP; }
            set { _mCSP = value; }
        }

        /// <summary>
        /// 原始字符串
        /// </summary>
        public string OriginalString
        {
            get { return _mstrOriginalString; }
            set { _mstrOriginalString = value; }
        }

        /// <summary>
        /// 加密后的字符
        /// </summary>
        public string EncryptedString
        {
            get { return _mstrEncryptedString; }
            set { _mstrEncryptedString = value; }
        }

        /// <summary>
        /// 对称加密算法密钥
        /// </summary>
        public byte[] key
        {
            get { return _mCSP.Key; }
            set { _mCSP.Key = value; }
        }

        /// <summary>
        /// 加密密钥
        /// </summary>
        public string KeyString
        {
            get { return Convert.ToBase64String(_mCSP.Key); }
            set { _mCSP.Key = Convert.FromBase64String(value); }
        }

        /// <summary>
        /// 初始化向量
        /// </summary>
        public byte[] IV
        {
            get { return _mCSP.IV; }
            set { _mCSP.IV = value; }
        }

        /// <summary>
        /// 初始化向量(Base64)
        /// </summary>
        public string IVString
        {
            get { return Convert.ToBase64String(_mCSP.IV); }
            set { _mCSP.IV = Convert.FromBase64String(value); }
        }

        #endregion

        #region "Encrypt() Methods"

        /// <summary>
        /// 进行对称加密
        /// </summary>
        public string Encrypt()
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = _mCSP.CreateEncryptor(_mCSP.Key, _mCSP.IV);

            byt = Encoding.Unicode.GetBytes(_mstrOriginalString);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            _mstrEncryptedString = Convert.ToBase64String(ms.ToArray());
            return _mstrEncryptedString;

        }

        /// <summary>
        /// 进行对称加密 -- 二次开发
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ivString"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public string Encrypt(string str, string ivString, string keyString)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(keyString);
            byte[] btIV = Encoding.UTF8.GetBytes(ivString);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.UTF8.GetBytes(str);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    _mstrEncryptedString = Convert.ToBase64String(ms.ToArray());
                    return _mstrEncryptedString;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 进行对称加密
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        public string Encrypt(string originalString)
        {
            _mstrOriginalString = originalString;

            return this.Encrypt();
        }

        /// <summary>
        /// 进行对称加密
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        /// <param name="encryptionType">加密类型</param>
        public string Encrypt(string originalString, SymmetricEncryptType encryptionType)
        {
            _mstrOriginalString = originalString;
            _mbytEncryptionType = encryptionType;

            return this.Encrypt();
        }

        #endregion

        #region "Decrypt() Methods"

        /// <summary>
        /// 进行对称解密
        /// </summary>
        public string Decrypt()
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            ct = _mCSP.CreateDecryptor(_mCSP.Key, _mCSP.IV);
            byt = Convert.FromBase64String(_mstrEncryptedString);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            _mstrOriginalString = Encoding.Unicode.GetString(ms.ToArray());
            return _mstrOriginalString;
        }

        /// <summary>
        /// 进行对称解密 -- 二次开发
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ivString"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public string Decrypt(string str, string ivString, string keyString)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(keyString);
            byte[] btIV = Encoding.UTF8.GetBytes(ivString);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(str);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    _mstrOriginalString = Encoding.UTF8.GetString(ms.ToArray());
                    return _mstrOriginalString;
                }
                catch
                {
                    return string.Empty;
                }
            }


            //MemoryStream ms = null;
            //CryptoStream cs = null;
            //StreamReader sr = null;

            //DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //try
            //{
            //    des.Key = ASCIIEncoding.ASCII.GetBytes(keyString);
            //    des.Mode = CipherMode.CBC;
            //    des.IV = Convert.FromBase64String(str.Substring(0, 12));
            //    //把密文头部12个字节长度的字符串取出作为DES解密用的初始化向量。

            //    byte[] inputByteArray = Convert.FromBase64String(str.Substring(12));
            //    ms = new MemoryStream();
            //    cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //    cs.Write(inputByteArray, 0, inputByteArray.Length);
            //    cs.FlushFinalBlock();
            //    _mstrOriginalString = Encoding.UTF8.GetString(ms.ToArray());
            //    return _mstrOriginalString;
            //}
            //finally
            //{
            //    if (sr != null) sr.Close();
            //    if (cs != null) cs.Close();
            //    if (ms != null) ms.Close();
            //}
        }


        /// <summary>
        /// 进行对称解密
        /// </summary>
        /// <param name="encryptedString">需要解密的字符串</param>
        public string Decrypt(string encryptedString)
        {
            _mstrEncryptedString = encryptedString;

            return this.Decrypt();
        }

        /// <summary>
        /// 进行对称解密
        /// </summary>
        /// <param name="encryptedString">需要解密的字符串</param>
        /// <param name="encryptionType">字符串加密类型</param>
        public string Decrypt(string encryptedString, SymmetricEncryptType encryptionType)
        {
            _mstrEncryptedString = encryptedString;
            _mbytEncryptionType = encryptionType;

            return this.Decrypt();
        }

        #endregion

        #region "SetEncryptor() Method"

        /// <summary>
        /// 设置加密算法
        /// </summary>
        private void SetEncryptor()
        {
            switch (_mbytEncryptionType)
            {
                case SymmetricEncryptType.DES:
                    _mCSP = new DESCryptoServiceProvider();
                    break;
                case SymmetricEncryptType.RC2:
                    _mCSP = new RC2CryptoServiceProvider();
                    break;
                case SymmetricEncryptType.Rijndael:
                    _mCSP = new RijndaelManaged();
                    break;
                case SymmetricEncryptType.TripleDES:
                    _mCSP = new TripleDESCryptoServiceProvider();
                    break;
            }

            // Generate Key
            _mCSP.GenerateKey();

            // Generate IV
            _mCSP.GenerateIV();
        }

        #endregion

        #region "Misc Public Methods"

        /// <summary>
        /// 生成随机密钥
        /// </summary>
        public string GenerateKey()
        {
            _mCSP.GenerateKey();
            return Convert.ToBase64String(_mCSP.Key);
        }

        /// <summary>
        /// 生成随机初始化向量
        /// </summary>
        public string GenerateIV()
        {
            _mCSP.GenerateIV();
            return Convert.ToBase64String(_mCSP.IV);
        }

        #endregion
    }

    /// <summary>
    /// 对称加密类型
    /// </summary>
    public enum SymmetricEncryptType : byte
    {
        /// <summary>
        /// DES算法
        /// </summary>
        DES,
        /// <summary>
        ///RC2算法 
        /// </summary>
        RC2,
        /// <summary>
        /// Rijndael算法
        /// </summary>
        Rijndael,
        /// <summary>
        /// TripleDES算法
        /// </summary>
        TripleDES
    }
}
