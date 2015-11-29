using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GPSInterfaces.Helpers
{
    public static class MD5Encoder
    {
        public static string EncodeMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }
    }
}
