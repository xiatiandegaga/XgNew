using Cloud.Utilities.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cloud.Utilities
{
    public class MerchantsBankAuthUtility
    {
        public static string GetParamSrc(string req)
        {
            var dicReq = JsonUtility.Deserialize<SortedDictionary<string, object>>(req);
            SortedDictionary<string, object> keyValues = new SortedDictionary<string, object>(dicReq);
            keyValues.OrderBy(m => m.Key, StringComparer.OrdinalIgnoreCase);//升序 把Key换成Value 就是对Value进行排序                                                                           //keyValues.OrderByDescending(m => m.Key);//降序
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, object> kv in keyValues)
            {
                string pkey = kv.Key;
                object pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }
            return str.ToString()[0..^1];
        }

        public static string GetSign(string req, string key = null)
        {
            var strToSign = GetParamSrc(req);
            // 假设已排序的待签名字符串为：strToSign
            //拼接支付密钥
            if (key != null)
                strToSign += "&" + key;
            //SHA-256签名
            return EncryptionUtility.SHA2(strToSign);
        }

        //招行验签
        public static bool IsValidSignature(string strToSign, string strSign, string publicKey)
        {
            byte[] bSource = Encoding.UTF8.GetBytes(strToSign);
            byte[] bSigdat = Convert.FromBase64String(strSign);
            byte[] bCert = null;
            try
            {
                bCert = Convert.FromBase64String(publicKey);
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"证书异常：{ex.Message}");
                return false;
            }

            try
            {
                RSACryptoServiceProvider rsa = DecodeX509PublicKey(bCert);
                if (!rsa.VerifyData(bSource, new SHA1CryptoServiceProvider(), bSigdat))
                {
                    Log.Logger.Error($"验证签名失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"验证签名失败：{ex.Message}");
                return false;
            }
            return true;
        }

        //------- Parses binary asn.1 X509 SubjectPublicKeyInfo; returns RSACryptoServiceProvider ---
        public static RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509key)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(x509key);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103)	//data read as little endian order (actual data order for Bit String is 03 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8203)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x00)		//expect null byte next
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102)	//data read as little endian order (actual data order for Integer is 02 81)
                    lowbyte = binr.ReadByte();	// read next bytes which is bytes in modulus
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte();	//advance 2 bytes
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);

                if (firstbyte == 0x00)
                {	//if first byte (highest order) of modulus is zero, don't include it
                    binr.ReadByte();	//skip this null byte
                    modsize -= 1;	//reduce modulus buffer size by 1
                }

                byte[] modulus = binr.ReadBytes(modsize);	//read the modulus bytes

                if (binr.ReadByte() != 0x02)			//expect an Integer for the exponent data
                    return null;
                int expbytes = (int)binr.ReadByte();		// should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = binr.ReadBytes(expbytes);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;
                RSA.ImportParameters(RSAKeyInfo);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
    }
}
