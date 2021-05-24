using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Fyk.C19.C19DataLoader
{
    static class Crypto
    {
        // https://fykutils.azurewebsites.net/api/Crypto?operation=encrypt&tocrypt=Hj1OChu6GTqbp6R58p2J;&tenant=35481dd1-663c-4a5f-9801-9396c6b35273
        private static string ReadUrlText(string url)
        {
            var rv = "";
            using (var client = new WebClient())
            {
                rv = client.DownloadString(url);
            }
            return rv;
        }
        public static string Decrypt(string plainText, Guid tenantId)
        {
            var rv = ReadUrlText(string.Format("https://fykutils.azurewebsites.net/api/Crypto?operation=decrypt&tocrypt={0}&tenant={1}", WebUtility.UrlEncode(plainText), tenantId));
            return rv;
        }
    }
}