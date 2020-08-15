using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarketWebAPI.Common
{
    public class ConfigurationHelper
    {
        private const string initVector = "pemgail9uzpgzl88";
        private const int keysize = 256;

        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static string DecryptConnectionString(string connectionString, string passPhrase)
        {
            string decryptedConnectionString;
            string encrypted, decrypted;

            int start, end;
            start = connectionString.IndexOf('[');
            if (start != -1)
            {
                end = connectionString.IndexOf(']', start);
                encrypted = connectionString.Substring(start + 1, end - start - 1);
                decrypted = DecryptString(encrypted, passPhrase);
                decryptedConnectionString = connectionString.Substring(0, start) + decrypted + connectionString.Substring(end + 1);
            }
            else
            {
                decryptedConnectionString = connectionString;
            }

            return decryptedConnectionString;
        }
    }
}
