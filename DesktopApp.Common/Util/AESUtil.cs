using System;
using System.IO;
using System.Security.Cryptography;

namespace DesktopApp.Common.Util
{
    public static class AESUtil
    {
        /// <summary>
        /// 生成aes的key和iv
        /// </summary>
        /// <returns></returns>
        public static Tuple<string, string> GenerateKeyIV()
        {
            using (var ase = Aes.Create())
            {
                return new Tuple<string, string>(Convert.ToBase64String(ase.Key), Convert.ToBase64String(ase.IV));
            }
        }

        /// <summary>
        /// 原文->base64密文
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Encryptase64String(string plainText, string key, string iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.IV = Convert.FromBase64String(iv);

                using (var ms = new MemoryStream())
                {
                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (var csEncrypt = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(csEncrypt))
                            {
                                sw.Write(plainText);
                                return Convert.ToBase64String(ms.ToArray());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// base64密文转原文
        /// </summary>
        /// <param name="ciperBase64Text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptBase64String(string ciperBase64Text, string key, string iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.IV = Convert.FromBase64String(iv);

                using (var ms = new MemoryStream(Convert.FromBase64String(ciperBase64Text)))
                {
                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        using (var csDecrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(csDecrypt))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}