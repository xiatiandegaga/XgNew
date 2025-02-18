using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using Cloud.Snowflake;
using System.Text;

namespace Cloud.Utilities
{
    /**
     * need lib:
     * BouncyCastle.Crypto.dll（http://www.bouncycastle.org/csharp/index.html） 
      
     * 用BC的注意点：
     * 这个版本的BC对SM3withSM2的结果为asn1格式的r和s，如果需要直接拼接的r||s需要自己转换。下面rsAsn1ToPlainByteArray、rsPlainByteArrayToAsn1就在干这事。
     * 这个版本的BC对SM2的结果为C1||C2||C3，据说为旧标准，新标准为C1||C3||C2，用新标准的需要自己转换。下面（被注释掉的）changeC1C2C3ToC1C3C2、changeC1C3C2ToC1C2C3就在干这事。java版的高版本有加上C1C3C2，csharp版没准以后也会加，但目前还没有，java版的目前可以初始化时“ SM2Engine sm2Engine = new SM2Engine(SM2Engine.Mode.C1C3C2);”。
     * 
     * 按要求国密算法仅允许使用加密机，本demo国密算法仅供学习使用，请不要用于生产用途。
     */
    public class SMSignUtility
    {

        //private static readonly ILog log = LogManager.GetLogger(typeof(SMSignUtility));

        private static X9ECParameters x9ECParameters = GMNamedCurves.GetByName("sm2p256v1");
        private static ECDomainParameters ecDomainParameters = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N);

        /**
         *
         * @param msg
         * @param userId
         * @param privateKey
         * @return r||s，直接拼接byte数组的rs
         */
        public static byte[] SignSm3WithSm2(byte[] msg, byte[] userId, AsymmetricKeyParameter privateKey)
        {
            return RsAsn1ToPlainByteArray(SignSm3WithSm2Asn1Rs(msg, userId, privateKey));
        }

        /**
          * @param msg
          * @param userId
          * @param privateKey
          * @return rs in <b>asn1 format</b>
          */
        public static byte[] SignSm3WithSm2Asn1Rs(byte[] msg, byte[] userId, AsymmetricKeyParameter privateKey)
        {
            try
            {
                ISigner signer = SignerUtilities.GetSigner("SM3withSM2");
                signer.Init(true, new ParametersWithID(privateKey, userId));
                signer.BlockUpdate(msg, 0, msg.Length);
                byte[] sig = signer.GenerateSignature();
                return sig;
            }
            catch (Exception e)
            {
                //log.Error("SignSm3WithSm2Asn1Rs error: " + e.Message, e);
                return null;
            }
        }

        /**
        *
        * @param msg
        * @param userId
        * @param rs r||s，直接拼接byte数组的rs
        * @param publicKey
        * @return
        */
        public static bool VerifySm3WithSm2(byte[] msg, byte[] userId, byte[] rs, AsymmetricKeyParameter publicKey)
        {
            if (rs == null || msg == null || userId == null) return false;
            if (rs.Length != RS_LEN * 2) return false;
            return VerifySm3WithSm2Asn1Rs(msg, userId, RsPlainByteArrayToAsn1(rs), publicKey);
        }

        /**
         *
         * @param msg
         * @param userId
         * @param rs in <b>asn1 format</b>
         * @param publicKey
         * @return
         */

        public static bool VerifySm3WithSm2Asn1Rs(byte[] msg, byte[] userId, byte[] sign, AsymmetricKeyParameter publicKey)
        {
            try
            {
                ISigner signer = SignerUtilities.GetSigner("SM3withSM2");
                signer.Init(false, new ParametersWithID(publicKey, userId));
                signer.BlockUpdate(msg, 0, msg.Length);
                return signer.VerifySignature(sign);
            }
            catch (Exception e)
            {
                //log.Error("VerifySm3WithSm2Asn1Rs error: " + e.Message, e);
                return false;
            }
        }

        /**
         * bc加解密使用旧标c1||c2||c3，此方法在加密后调用，将结果转化为c1||c3||c2
         * @param c1c2c3
         * @return
         */
        private static byte[] ChangeC1C2C3ToC1C3C2(byte[] c1c2c3)
        {
            int c1Len = (x9ECParameters.Curve.FieldSize + 7) / 8 * 2 + 1; //sm2p256v1的这个固定65。可看GMNamedCurves、ECCurve代码。
            const int c3Len = 32; //new SM3Digest().getDigestSize();
            byte[] result = new byte[c1c2c3.Length];
            Buffer.BlockCopy(c1c2c3, 0, result, 0, c1Len); //c1
            Buffer.BlockCopy(c1c2c3, c1c2c3.Length - c3Len, result, c1Len, c3Len); //c3
            Buffer.BlockCopy(c1c2c3, c1Len, result, c1Len + c3Len, c1c2c3.Length - c1Len - c3Len); //c2
            return result;
        }


        /**
         * bc加解密使用旧标c1||c3||c2，此方法在解密前调用，将密文转化为c1||c2||c3再去解密
         * @param c1c3c2
         * @return
         */
        private static byte[] ChangeC1C3C2ToC1C2C3(byte[] c1c3c2)
        {
            int c1Len = (x9ECParameters.Curve.FieldSize + 7) / 8 * 2 + 1; //sm2p256v1的这个固定65。可看GMNamedCurves、ECCurve代码。
            const int c3Len = 32; //new SM3Digest().GetDigestSize();
            byte[] result = new byte[c1c3c2.Length];
            Buffer.BlockCopy(c1c3c2, 0, result, 0, c1Len); //c1: 0->65
            Buffer.BlockCopy(c1c3c2, c1Len + c3Len, result, c1Len, c1c3c2.Length - c1Len - c3Len); //c2
            Buffer.BlockCopy(c1c3c2, c1Len, result, c1c3c2.Length - c3Len, c3Len); //c3
            return result;
        }

        /**
         * c1||c3||c2
         * @param data
         * @param key
         * @return
         */
        public static byte[] Sm2Decrypt(byte[] data, AsymmetricKeyParameter key)
        {
            return Sm2DecryptOld(ChangeC1C3C2ToC1C2C3(data), key);
        }

        /**
         * c1||c3||c2
         * @param data
         * @param key
         * @return
         */

        public static byte[] Sm2Encrypt(byte[] data, AsymmetricKeyParameter key)
        {
            return ChangeC1C2C3ToC1C3C2(Sm2EncryptOld(data, key));
        }

        /**
         * c1||c2||c3
         * @param data
         * @param key
         * @return
         */
        public static byte[] Sm2EncryptOld(byte[] data, AsymmetricKeyParameter pubkey)
        {
            try
            {
                SM2Engine sm2Engine = new SM2Engine();
                sm2Engine.Init(true, new ParametersWithRandom(pubkey, new SecureRandom()));
                return sm2Engine.ProcessBlock(data, 0, data.Length);
            }
            catch (Exception e)
            {
                //log.Error("Sm2EncryptOld error: " + e.Message, e);
                return null;
            }
        }

        /**
         * c1||c2||c3
         * @param data
         * @param key
         * @return
         */
        public static byte[] Sm2DecryptOld(byte[] data, AsymmetricKeyParameter key)
        {
            try
            {
                SM2Engine sm2Engine = new SM2Engine();
                sm2Engine.Init(false, key);
                return sm2Engine.ProcessBlock(data, 0, data.Length);
            }
            catch (Exception e)
            {
                //log.Error("Sm2DecryptOld error: " + e.Message, e);
                return null;
            }
        }

        /**
         * @param bytes
         * @return
         */
        public static byte[] Sm3(byte[] bytes)
        {
            try
            {
                SM3Digest digest = new SM3Digest();
                digest.BlockUpdate(bytes, 0, bytes.Length);
                byte[] result = DigestUtilities.DoFinal(digest);
                return result;
            }
            catch (Exception e)
            {
                //log.Error("Sm3 error: " + e.Message, e);
                return null;
            }
        }

        private const int RS_LEN = 32;

        private static byte[] BigIntToFixexLengthBytes(BigInteger rOrS)
        {
            // for sm2p256v1, n is 00fffffffeffffffffffffffffffffffff7203df6b21c6052b53bbf40939d54123,
            // r and s are the result of mod n, so they should be less than n and have length<=32
            byte[] rs = rOrS.ToByteArray();
            if (rs.Length == RS_LEN) return rs;
            else if (rs.Length == RS_LEN + 1 && rs[0] == 0) return Arrays.CopyOfRange(rs, 1, RS_LEN + 1);
            else if (rs.Length < RS_LEN)
            {
                byte[] result = new byte[RS_LEN];
                Arrays.Fill(result, (byte)0);
                Buffer.BlockCopy(rs, 0, result, RS_LEN - rs.Length, rs.Length);
                return result;
            }
            else
            {
                throw new ArgumentException("err rs: " + Hex.ToHexString(rs));
            }
        }

        /**
         * BC的SM3withSM2签名得到的结果的rs是asn1格式的，这个方法转化成直接拼接r||s
         * @param rsDer rs in asn1 format
         * @return sign result in plain byte array
         */
        private static byte[] RsAsn1ToPlainByteArray(byte[] rsDer)
        {
            Asn1Sequence seq = Asn1Sequence.GetInstance(rsDer);
            byte[] r = BigIntToFixexLengthBytes(DerInteger.GetInstance(seq[0]).Value);
            byte[] s = BigIntToFixexLengthBytes(DerInteger.GetInstance(seq[1]).Value);
            byte[] result = new byte[RS_LEN * 2];
            Buffer.BlockCopy(r, 0, result, 0, r.Length);
            Buffer.BlockCopy(s, 0, result, RS_LEN, s.Length);
            return result;
        }

        /**
         * BC的SM3withSM2验签需要的rs是asn1格式的，这个方法将直接拼接r||s的字节数组转化成asn1格式
         * @param sign in plain byte array
         * @return rs result in asn1 format
         */
        private static byte[] RsPlainByteArrayToAsn1(byte[] sign)
        {
            if (sign.Length != RS_LEN * 2) throw new ArgumentException("err rs. ");
            BigInteger r = new BigInteger(1, Arrays.CopyOfRange(sign, 0, RS_LEN));
            BigInteger s = new BigInteger(1, Arrays.CopyOfRange(sign, RS_LEN, RS_LEN * 2));
            Asn1EncodableVector v = new Asn1EncodableVector();
            v.Add(new DerInteger(r));
            v.Add(new DerInteger(s));
            try
            {
                return new DerSequence(v).GetEncoded("DER");
            }
            catch (IOException e)
            {
                //log.Error("RsPlainByteArrayToAsn1 error: " + e.Message, e);
                return null;
            }
        }

        public static AsymmetricCipherKeyPair GenerateKeyPair()
        {
            try
            {
                ECKeyPairGenerator kpGen = new ECKeyPairGenerator();
                kpGen.Init(new ECKeyGenerationParameters(ecDomainParameters, new SecureRandom()));
                return kpGen.GenerateKeyPair();
            }
            catch (Exception e)
            {
                //log.Error("generateKeyPair error: " + e.Message, e);
                return null;
            }
        }

        public static ECPrivateKeyParameters GetPrivatekeyFromD(BigInteger d)
        {
            return new ECPrivateKeyParameters(d, ecDomainParameters);
        }

        public static ECPublicKeyParameters GetPublickeyFromXY(BigInteger x, BigInteger y)
        {
            return new ECPublicKeyParameters(x9ECParameters.Curve.CreatePoint(x, y), ecDomainParameters);
        }

        public static AsymmetricKeyParameter GetPublickeyFromX509File(FileInfo file)
        {

            FileStream fileStream = null;
            try
            {
                //file.DirectoryName + "\\" + file.Name
                fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                X509Certificate certificate = new X509CertificateParser().ReadCertificate(fileStream);
                return certificate.GetPublicKey();
            }
            catch (Exception e)
            {
                //log.Error(file.Name + "读取失败，异常：" + e);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return null;
        }

        public class Sm2Cert
        {
            public AsymmetricKeyParameter privateKey;
            public AsymmetricKeyParameter publicKey;
            public String certId;
        }

        private static byte[] ToByteArray(int i)
        {
            byte[] byteArray = new byte[4];
            byteArray[0] = (byte)(i >> 24);
            byteArray[1] = (byte)((i & 0xFFFFFF) >> 16);
            byteArray[2] = (byte)((i & 0xFFFF) >> 8);
            byteArray[3] = (byte)(i & 0xFF);
            return byteArray;
        }

        /**
         * 字节数组拼接
         *
         * @param params
         * @return
         */
        private static byte[] Join(params byte[][] byteArrays)
        {
            List<byte> byteSource = new List<byte>();
            for (int i = 0; i < byteArrays.Length; i++)
            {
                byteSource.AddRange(byteArrays[i]);
            }
            byte[] data = byteSource.ToArray();
            return data;
        }

        /**
         * 密钥派生函数
         *
         * @param Z
         * @param klen
         *            生成klen字节数长度的密钥
         * @return
         */
        private static byte[] KDF(byte[] Z, int klen)
        {
            int ct = 1;
            int end = (int)Math.Ceiling(klen * 1.0 / 32);
            List<byte> byteSource = new List<byte>();
            try
            {
                for (int i = 1; i < end; i++)
                {
                    byteSource.AddRange(SMSignUtility.Sm3(Join(Z, ToByteArray(ct))));
                    ct++;
                }
                byte[] last = SMSignUtility.Sm3(Join(Z, ToByteArray(ct)));
                if (klen % 32 == 0)
                {
                    byteSource.AddRange(last);
                }
                else
                    byteSource.AddRange(Arrays.CopyOfRange(last, 0, klen % 32));
                return byteSource.ToArray();
            }
            catch (Exception e)
            {
                //log.Error("KDF error: " + e.Message, e);
            }
            return null;
        }

        public static byte[] Sm4DecryptCBC(byte[] keyBytes, byte[] cipher, byte[] iv, String algo)
        {
            if (keyBytes.Length != 16) throw new ArgumentException("err key length");
            if (cipher.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

            try
            {
                KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
                IBufferedCipher c = CipherUtilities.GetCipher(algo);
                if (iv == null) iv = ZeroIv(algo);
                c.Init(false, new ParametersWithIV(key, iv));
                return c.DoFinal(cipher);
            }
            catch (Exception e)
            {
                //log.Error("Sm4DecryptCBC error: " + e.Message, e);
                return null;
            }
        }


        public static byte[] Sm4EncryptCBC(byte[] keyBytes, byte[] plain, byte[] iv, String algo)
        {
            if (keyBytes.Length != 16) throw new ArgumentException("err key length");
            if (plain.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

            try
            {
                KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
                IBufferedCipher c = CipherUtilities.GetCipher(algo);
                if (iv == null) iv = ZeroIv(algo);
                c.Init(true, new ParametersWithIV(key, iv));
                return c.DoFinal(plain);
            }
            catch (Exception e)
            {
                //log.Error("Sm4EncryptCBC error: " + e.Message, e);
                return null;
            }
        }


        public static byte[] Sm4EncryptECB(byte[] keyBytes, byte[] plain, string algo)
        {
            if (keyBytes.Length != 16) throw new ArgumentException("err key length");
            //NoPadding 的情况下需要校验数据长度是16的倍数.
            if (plain.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

            try
            {
                KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
                IBufferedCipher c = CipherUtilities.GetCipher(algo);
                c.Init(true, key);
                return c.DoFinal(plain);
            }
            catch (Exception e)
            {
                //log.Error("Sm4EncryptECB error: " + e.Message, e);
                return null;
            }
        }

        public static byte[] Sm4DecryptECB(byte[] keyBytes, byte[] cipher, string algo)
        {
            if (keyBytes.Length != 16) throw new ArgumentException("err key length");
            if (cipher.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

            try
            {
                KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
                IBufferedCipher c = CipherUtilities.GetCipher(algo);
                c.Init(false, key);
                return c.DoFinal(cipher);
            }
            catch (Exception e)
            {
                //log.Error("Sm4DecryptECB error: " + e.Message, e);
                return null;
            }
        }

        public const String SM4_ECB_NOPADDING = "SM4/ECB/NoPadding";
        public const String SM4_CBC_NOPADDING = "SM4/CBC/NoPadding";
        public const String SM4_CBC_PKCS7PADDING = "SM4/CBC/PKCS7Padding";

        /**
         * cfca官网CSP沙箱导出的sm2文件
         * @param pem 二进制原文
         * @param pwd 密码
         * @return
         */
        public static Sm2Cert readSm2File(byte[] pem, String pwd)
        {

            Sm2Cert sm2Cert = new Sm2Cert();
            try
            {
                Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray(pem);
                //            ASN1Integer asn1Integer = (ASN1Integer) asn1Sequence.getObjectAt(0); //version=1
                Asn1Sequence priSeq = (Asn1Sequence)asn1Sequence[1];//private key
                Asn1Sequence pubSeq = (Asn1Sequence)asn1Sequence[2];//public key and x509 cert

                //            ASN1ObjectIdentifier sm2DataOid = (ASN1ObjectIdentifier) priSeq.getObjectAt(0);
                //            ASN1ObjectIdentifier sm4AlgOid = (ASN1ObjectIdentifier) priSeq.getObjectAt(1);
                Asn1OctetString priKeyAsn1 = (Asn1OctetString)priSeq[2];
                byte[] key = KDF(System.Text.Encoding.UTF8.GetBytes(pwd), 32);
                byte[] priKeyD = Sm4DecryptCBC(Arrays.CopyOfRange(key, 16, 32),
                        priKeyAsn1.GetOctets(),
                        Arrays.CopyOfRange(key, 0, 16), SM4_CBC_PKCS7PADDING);
                sm2Cert.privateKey = GetPrivatekeyFromD(new BigInteger(1, priKeyD));
                //            log.Info(Hex.toHexString(priKeyD));

                //            ASN1ObjectIdentifier sm2DataOidPub = (ASN1ObjectIdentifier) pubSeq.getObjectAt(0);
                Asn1OctetString pubKeyX509 = (Asn1OctetString)pubSeq[1];
                X509Certificate x509 = (X509Certificate)new X509CertificateParser().ReadCertificate(pubKeyX509.GetOctets());
                sm2Cert.publicKey = x509.GetPublicKey();
                sm2Cert.certId = x509.SerialNumber.ToString(10); //这里转10进账，有啥其他进制要求的自己改改
                return sm2Cert;
            }
            catch (Exception e)
            {
                //log.Error("readSm2File error: " + e.Message, e);
                return null;
            }
        }

        /**
         *
         * @param cert
         * @return
         */
        public static Sm2Cert ReadSm2X509Cert(byte[] cert)
        {
            Sm2Cert sm2Cert = new Sm2Cert();
            try
            {

                X509Certificate x509 = new X509CertificateParser().ReadCertificate(cert);
                sm2Cert.publicKey = x509.GetPublicKey();
                sm2Cert.certId = x509.SerialNumber.ToString(10); //这里转10进账，有啥其他进制要求的自己改改
                return sm2Cert;
            }
            catch (Exception e)
            {
                //log.Error("ReadSm2X509Cert error: " + e.Message, e);
                return null;
            }
        }

        public static byte[] ZeroIv(String algo)
        {

            try
            {
                IBufferedCipher cipher = CipherUtilities.GetCipher(algo);
                int blockSize = cipher.GetBlockSize();
                byte[] iv = new byte[blockSize];
                Arrays.Fill(iv, (byte)0);
                return iv;
            }
            catch (Exception e)
            {
                //log.Error("ZeroIv error: " + e.Message, e);
                return null;
            }
        }

        public static string Sm2DecryptString(string privateKeyHex,string signData)
        {
            BigInteger d = new BigInteger(privateKeyHex, 16);
            AsymmetricKeyParameter bcecPrivateKey = GetPrivatekeyFromD(d);
            byte[] byToDecrypt = Hex.Decode(signData);
            byte[] byDecrypted = Sm2Decrypt(byToDecrypt, bcecPrivateKey);
            var  strDecrypted = Encoding.UTF8.GetString(byDecrypted);
            return strDecrypted;
        }
        public static void Test(string[] s)
        {

            // 随便看看
            //log.Info("GMNamedCurves: ");
            foreach (string e in GMNamedCurves.Names)
            {
                //log.Info(e);
            }
            //log.Info("sm2p256v1 n:" + x9ECParameters.N);
            //log.Info("sm2p256v1 nHex:" + Hex.ToHexString(x9ECParameters.N.ToByteArray()));

            // 生成公私钥对 ---------------------
            AsymmetricCipherKeyPair kp = SMSignUtility.GenerateKeyPair();
            //log.Info("private key d: " + ((ECPrivateKeyParameters)kp.Private).D);
            //log.Info("public key q:" + ((ECPublicKeyParameters)kp.Public).Q); //{x, y, zs...}

            //签名验签
            byte[] msg = System.Text.Encoding.UTF8.GetBytes("message digest");
            byte[] userId = System.Text.Encoding.UTF8.GetBytes("userId");
            byte[] sig = SignSm3WithSm2(msg, userId, kp.Private);
            //log.Info("testSignSm3WithSm2: " + Hex.ToHexString(sig));
            //log.Info("testVerifySm3WithSm2: " + VerifySm3WithSm2(msg, userId, sig, kp.Public));

            // 由d生成私钥 ---------------------
            BigInteger d = new BigInteger("097b5230ef27c7df0fa768289d13ad4e8a96266f0fcb8de40d5942af4293a54a", 16);
            ECPrivateKeyParameters bcecPrivateKey = GetPrivatekeyFromD(d);
            //log.Info("testGetFromD: " + bcecPrivateKey.D.ToString(16));

            //公钥X坐标PublicKeyXHex: 59cf9940ea0809a97b1cbffbb3e9d96d0fe842c1335418280bfc51dd4e08a5d4
            //公钥Y坐标PublicKeyYHex: 9a7f77c578644050e09a9adc4245d1e6eba97554bc8ffd4fe15a78f37f891ff8
            AsymmetricKeyParameter publicKey = GetPublickeyFromX509File(new FileInfo("d:/certs/69629141652.cer"));
            //log.Info(publicKey);
            AsymmetricKeyParameter publicKey1 = GetPublickeyFromXY(new BigInteger("59cf9940ea0809a97b1cbffbb3e9d96d0fe842c1335418280bfc51dd4e08a5d4", 16), new BigInteger("9a7f77c578644050e09a9adc4245d1e6eba97554bc8ffd4fe15a78f37f891ff8", 16));
            //log.Info("testReadFromX509File: " + ((ECPublicKeyParameters)publicKey).Q);
            //log.Info("testGetFromXY: " + ((ECPublicKeyParameters)publicKey1).Q);
            //log.Info("testPubKey: " + publicKey.Equals(publicKey1));
            //log.Info("testPubKey: " + ((ECPublicKeyParameters)publicKey).Q.Equals(((ECPublicKeyParameters)publicKey1).Q));

            // sm2 encrypt and decrypt test ---------------------
            AsymmetricCipherKeyPair kp2 = GenerateKeyPair();
            AsymmetricKeyParameter publicKey2 = kp2.Public;
            AsymmetricKeyParameter privateKey2 = kp2.Private;
            byte[] bs = Sm2Encrypt(System.Text.Encoding.UTF8.GetBytes("s"), publicKey2);
            //log.Info("testSm2Enc dec: " + Hex.ToHexString(bs));
            bs = Sm2Decrypt(bs, privateKey2);
            //log.Info("testSm2Enc dec: " + System.Text.Encoding.UTF8.GetString(bs));

            // sm4 encrypt and decrypt test ---------------------
            //0123456789abcdeffedcba9876543210 + 0123456789abcdeffedcba9876543210 -> 681edf34d206965e86b3e94f536e4246
            byte[] plain = Hex.Decode("0123456789abcdeffedcba98765432100123456789abcdeffedcba98765432100123456789abcdeffedcba9876543210");
            byte[] key = Hex.Decode("0123456789abcdeffedcba9876543210");
            byte[] cipher = Hex.Decode("595298c7c6fd271f0402f804c33d3f66");
            bs = Sm4EncryptECB(key, plain, SMSignUtility.SM4_ECB_NOPADDING);
            //log.Info("testSm4EncEcb: " + Hex.ToHexString(bs)); ;
            bs = Sm4DecryptECB(key, bs, SMSignUtility.SM4_ECB_NOPADDING);
            //log.Info("testSm4DecEcb: " + Hex.ToHexString(bs));

            //读.sm2文件
            String sm2 = "MIIDHQIBATBHBgoqgRzPVQYBBAIBBgcqgRzPVQFoBDDW5/I9kZhObxXE9Vh1CzHdZhIhxn+3byBU\nUrzmGRKbDRMgI3hJKdvpqWkM5G4LNcIwggLNBgoqgRzPVQYBBAIBBIICvTCCArkwggJdoAMCAQIC\nBRA2QSlgMAwGCCqBHM9VAYN1BQAwXDELMAkGA1UEBhMCQ04xMDAuBgNVBAoMJ0NoaW5hIEZpbmFu\nY2lhbCBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTEbMBkGA1UEAwwSQ0ZDQSBURVNUIFNNMiBPQ0Ex\nMB4XDTE4MTEyNjEwMTQxNVoXDTIwMTEyNjEwMTQxNVowcjELMAkGA1UEBhMCY24xEjAQBgNVBAoM\nCUNGQ0EgT0NBMTEOMAwGA1UECwwFQ1VQUkExFDASBgNVBAsMC0VudGVycHJpc2VzMSkwJwYDVQQD\nDCAwNDFAWnRlc3RAMDAwMTAwMDA6U0lHTkAwMDAwMDAwMTBZMBMGByqGSM49AgEGCCqBHM9VAYIt\nA0IABDRNKhvnjaMUShsM4MJ330WhyOwpZEHoAGfqxFGX+rcL9x069dyrmiF3+2ezwSNh1/6YqfFZ\nX9koM9zE5RG4USmjgfMwgfAwHwYDVR0jBBgwFoAUa/4Y2o9COqa4bbMuiIM6NKLBMOEwSAYDVR0g\nBEEwPzA9BghggRyG7yoBATAxMC8GCCsGAQUFBwIBFiNodHRwOi8vd3d3LmNmY2EuY29tLmNuL3Vz\nL3VzLTE0Lmh0bTA4BgNVHR8EMTAvMC2gK6AphidodHRwOi8vdWNybC5jZmNhLmNvbS5jbi9TTTIv\nY3JsNDI4NS5jcmwwCwYDVR0PBAQDAgPoMB0GA1UdDgQWBBREhx9VlDdMIdIbhAxKnGhPx8FcHDAd\nBgNVHSUEFjAUBggrBgEFBQcDAgYIKwYBBQUHAwQwDAYIKoEcz1UBg3UFAANIADBFAiEAgWvQi3h6\niW4jgF4huuXfhWInJmTTYr2EIAdG8V4M8fYCIBixygdmfPL9szcK2pzCYmIb6CBzo5SMv50Odycc\nVfY6";
            bs = Convert.FromBase64String(sm2);
            String pwd = "cfca1234";
            SMSignUtility.Sm2Cert sm2Cert = SMSignUtility.readSm2File(bs, pwd);
            //log.Info("testReadSm2File, pubkey: " + ((ECPublicKeyParameters)sm2Cert.publicKey).Q.ToString());
            //log.Info("testReadSm2File, prikey: " + Hex.ToHexString(((ECPrivateKeyParameters)sm2Cert.privateKey).D.ToByteArray()));
            //log.Info("testReadSm2File, certId: " + sm2Cert.certId);

            bs = Sm2Encrypt(System.Text.Encoding.UTF8.GetBytes("s"), ((ECPublicKeyParameters)sm2Cert.publicKey));
            //log.Info("testSm2Enc dec: " + Hex.ToHexString(bs));
            bs = Sm2Decrypt(bs, ((ECPrivateKeyParameters)sm2Cert.privateKey));
            //log.Info("testSm2Enc dec: " + System.Text.Encoding.UTF8.GetString(bs));

            msg = System.Text.Encoding.UTF8.GetBytes("message digest");
            userId = System.Text.Encoding.UTF8.GetBytes("userId");
            sig = SignSm3WithSm2(msg, userId, ((ECPrivateKeyParameters)sm2Cert.privateKey));
            //log.Info("testSignSm3WithSm2: " + Hex.ToHexString(sig));
            //log.Info("testVerifySm3WithSm2: " + VerifySm3WithSm2(msg, userId, sig, ((ECPublicKeyParameters)sm2Cert.publicKey)));
        }

    }
}