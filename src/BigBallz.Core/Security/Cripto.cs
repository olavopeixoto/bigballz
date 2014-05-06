using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace BigBallz.Core.Security
{

    /// <summary>
    /// Interface do package responsável pela criptografia de informações do sistema
    /// </summary>
    public class Cripto
    {
        /// <summary>
        /// Encripta um texto utilizando uma chave padrão de 64 bits 
        /// </summary>
        /// <param name="texto">Texto a ser criptografado</param>
        /// <returns>Texto criptografado</returns>
        public static string Encriptar(string texto)
        {
            return Encriptar(texto, "4Jkw9N3f");
        }

        /// <summary>
        /// Encripta um texto utilizando uma chave de 64 bits
        /// </summary>
        /// <param name="texto">Texto a ser criptografado</param>
        /// <param name="chavePrivada">Chave de 64 bits</param>
        /// <returns>Texto criptografado</returns>
        public static string Encriptar(string texto, string chavePrivada)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray;

            if (texto != null)
            {
                inputByteArray = Encoding.UTF8.GetBytes(texto);

                //Create the crypto objects, with the key, as passed in
                des.Key = Encoding.ASCII.GetBytes(chavePrivada);
                des.IV = Encoding.ASCII.GetBytes(chavePrivada.Substring(0, 8));
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        //Write the byte array into the crypto stream
                        //(It will end up in the memory stream)
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();

                        //Get the data back from the memory stream, and into a string
                        var ret = new StringBuilder();

                        foreach (byte b in ms.ToArray())
                        {
                            //Format as hex
                            ret.AppendFormat("{0:X2}", b);
                        }
                        return ret.ToString();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Decriptografa um texto criptografado utilizando uma chave de 64 bits
        /// </summary>
        /// <param name="textoEncriptado">Texto criptografado</param>
        /// <param name="chavePrivada">Chave de 64 bits</param>
        /// <returns>Texto descriptografado</returns>
        public string Decriptar(string textoEncriptado, string chavePrivada)
        {
            //Get the data back from the memory stream, and into a string
            var inputByteArray = new byte[(textoEncriptado.Length / 2)];

            //Put the input string into the byte array
            for (int x = 0; x <= ((textoEncriptado.Length / 2) - 1); x++)
            {
                inputByteArray[x] = Convert.ToByte(textoEncriptado.Substring(x * 2, 2), 16);
            }

            //Create the crypto objects
            var des = new DESCryptoServiceProvider
                          {
                              Key = Encoding.UTF8.GetBytes(chavePrivada),
                              IV = Encoding.UTF8.GetBytes(chavePrivada.Substring(0, 8))
                          };
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {

                    ms.Write(inputByteArray, 0, inputByteArray.Length);
                    ms.Position = 0;

                    return new StreamReader(cs).ReadToEnd();
                }
            }
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

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        protected static string Decrypt(ICryptoTransform cryptoTransform, string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            var inputByteArray = Convert.FromBase64String(text);

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

        /// <summary>
        /// Encripta um texto utilizando uma chave de 64 bits
        /// </summary>
        /// <param name="texto">Texto a ser criptografado</param>
        /// <param name="chavePrivada">Chave de 64 bits</param>
        /// <returns>Texto criptografado</returns>
        public static string EncriptarDES(string texto, string chavePrivada)
        {
            var des = new DESCryptoServiceProvider()
            {
                Key = Encoding.UTF8.GetBytes(chavePrivada),
                IV = Encoding.UTF8.GetBytes(chavePrivada.Substring(0, 8))
            };

            return Encrypt(des.CreateEncryptor(), texto);
        }

        /// <summary>
        /// Decriptografa um texto criptografado utilizando uma chave de 64 bits
        /// </summary>
        /// <param name="textoEncriptado">Texto criptografado</param>
        /// <param name="chavePrivada">Chave de 64 bits</param>
        /// <returns>Texto descriptografado</returns>
        public static string DecriptarDES(string textoEncriptado, string chavePrivada)
        {
            var des = new DESCryptoServiceProvider()
                          {
                              Key = Encoding.UTF8.GetBytes(chavePrivada),
                              IV = Encoding.UTF8.GetBytes(chavePrivada.Substring(0, 8))
                          };

            return Decrypt(des.CreateDecryptor(), textoEncriptado);
        }

        /// <summary>
        /// Encripta um texto utilizando uma chave de 128 bits
        /// </summary>
        /// <param name="texto">Texto a ser criptografado</param>
        /// <param name="chavePrivada">Chave de 128 bits</param>
        /// <returns>Texto criptografado</returns>
        public string EncriptarRC2(string texto, string chavePrivada)
        {
            var rc = new RC2CryptoServiceProvider
                         {
                             Key = Encoding.UTF8.GetBytes(chavePrivada),
                             IV = Encoding.UTF8.GetBytes(chavePrivada.Substring(0, 8))
                         };
            return Encrypt(rc.CreateEncryptor(), texto);
        }

        /// <summary>
        /// Encripta um texto utilizando algoritmo MD5
        /// </summary>
        /// <param name="texto">Texto a ser criptografado</param>
        /// <returns>Texto criptografado</returns>
        public string EncriptarMD5(string texto)
        {
            // converte o texto para um byte array
            var data1ToHash = Encoding.Unicode.GetBytes(texto);

            // Create hash value from String 1 using MD5 instance returned by Crypto Config system
            byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(data1ToHash);

            return (BitConverter.ToString(hashvalue1));
        }

        /// <summary>
        /// Encripta um arquivo utilizando chave de 128 bits
        /// </summary>
        /// <param name="arquivo">Nome do arquivo a ser criptografado</param>
        /// <param name="chavePrivada">Chave de 128 bits</param>
        public void EncriptarArquivoRC2(string arquivo, string chavePrivada)
        {
            byte[] key = Encoding.ASCII.GetBytes(chavePrivada);

            var fsCrypt = new FileStream(arquivo + ".Crypt", FileMode.Create);

            var RMCrypto = new RC2CryptoServiceProvider();

            var cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);

            var fsIn = new FileStream(arquivo, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1) cs.WriteByte((byte)data);

            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }

        /// <summary>
        /// Decriptografa um texto criptografado utilizando uma chave padrão de 64 bits
        /// </summary>
        /// <param name="textoEncriptado">Texto criptografado</param>
        /// <returns>Texto descriptografado</returns>
        public string Decriptar(string textoEncriptado)
        {
            return Decriptar(textoEncriptado, "4Jkw9N3f");
        }

        /// <summary>
        /// Decriptografa um texto criptografado utilizando uma chave de 128 bits
        /// </summary>
        /// <param name="textoEncriptado">Texto criptografado</param>
        /// <param name="chavePrivada">Chave de 128 bits</param>
        /// <returns>Texto descriptografado</returns>
        public string DecriptarRC2(string textoEncriptado, string chavePrivada)
        {
            var rc = new RC2CryptoServiceProvider
                         {
                             Key = Encoding.ASCII.GetBytes(chavePrivada),
                             IV = Encoding.ASCII.GetBytes(chavePrivada.Substring(0, 8))
                         };
            return Decrypt(rc.CreateDecryptor(), textoEncriptado);
        }

        /// <summary>
        /// Decriptografa um arquivo utilizando chave de 128 bits
        /// </summary>
        /// <param name="arquivo">Nome do arquivo a ser criptografado</param>
        /// <param name="chavePrivada">Chave de 128 bits</param>
        public void DecriptarArquivoRC2(string arquivo, string chavePrivada)
        {
            var key = Encoding.ASCII.GetBytes(chavePrivada);

            using (var fsCrypt = new FileStream(arquivo, FileMode.Open))
            {

                var RMCrypto = new RC2CryptoServiceProvider();

                var cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

                var fsOut = new FileStream(arquivo + ".orig", FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1) fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
            }
        }

        public static string defaultHash(string text)
        {
            return new Cripto().EncriptarMD5(text);
        }

        public static string defaultCrypt(string text)
        {
            return Encriptar(text);
        }

        public static string defaultDecrypt(string text)
        {
            return new Cripto().Decriptar(text);
        }

        public static string defaultCrypt(string text, string key)
        {
            return Encriptar(text, key);
        }

        public static string defaultDecrypt(string text, string key)
        {
            return new Cripto().Decriptar(text, key);
        }
    }
}
