using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BigBallz.Helpers
{
    public static class CryptHelper
    {
        private static readonly string Key = ConfigurationManager.AppSettings["crypto-key"];
        
        public static string EncryptAES256(string text)
        {
            var aes = new AesCryptoServiceProvider()
            {
                KeySize = 256,
                BlockSize = 128,
                Key = Encoding.UTF8.GetBytes(Key),
                IV = Encoding.ASCII.GetBytes(Key)
            };

            return Encrypt(aes.CreateEncryptor(), text);
        }

        public static string DecryptAES256(string text)
        {
            var rc = new AesCryptoServiceProvider
            {
                KeySize = 256,
                BlockSize = 128,
                Key = Encoding.UTF8.GetBytes(Key),
                IV = Encoding.ASCII.GetBytes(Key)
            };
            return Decrypt(rc.CreateDecryptor(), text);
        }

        public static string Encrypt(ICryptoTransform cryptoTransform, string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var inputByteArray = Encoding.UTF8.GetBytes(text);

            using (var ms = new MemoryStream(1024))
            {
                using (var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    //Write the byte array into the crypto stream
                    //(It will end up in the memory stream)
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();

                    return BitConverter.ToString(ms.ToArray()).Replace("-", string.Empty);
                    //return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(ICryptoTransform cryptoTransform, string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            //var inputByteArray = Convert.FromBase64String(text);
            var inputByteArray = HexStringToByteArray(text);

            using (var ms = new MemoryStream(inputByteArray.Length))
            {
                using (var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Read))
                {
                    ms.Write(inputByteArray, 0, inputByteArray.Length);
                    ms.Position = 0;

                    return new StreamReader(cs).ReadToEnd();
                }
            }
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            var value = new Byte[hexString.Length / 2];
            for(var index = 0; index < value.Length; index++)
            {
                Byte.TryParse(hexString.Substring(index * 2, 2), System.Globalization.NumberStyles.HexNumber, null, out value[index]);
            }

            return value;
        }
    }
}