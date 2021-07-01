using System;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;


namespace SourceTwo.Services
{

    public class HmacAuthenticationService : IHmacAuthenticationService
    {
        private string _applicationId = "XYZ-123";
        private string _secretKey = "MYSECRETKEY1";

        public bool IsValidRequest(string authHeader, string authHeaderValue, string method, string uri)
        {
            try
            {
                if (authHeader != "hmac")
                {
                    return false;
                }

                if (authHeaderValue == null || String.IsNullOrEmpty(authHeaderValue))
                {
                    return false;
                }

                string tokenString = authHeaderValue;
                string[] authenticationParameters =
                   tokenString.Split(':');

                if (authenticationParameters.Length != 4)
                {
                    return false;
                }

                if (!authenticationParameters[0].Equals(_applicationId,
                      StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (MemoryCache.Default.Contains(authenticationParameters[2]))
                {
                    return false;
                }

                if ((DateTime.UtcNow - DateTime.ParseExact(authenticationParameters[3], "dd/MM/yyyy HH-mm-ss", null)).
                      Seconds > 600)
                {
                    return false;
                }

                MemoryCache.Default.Add(authenticationParameters[2],
                   authenticationParameters[3], DateTimeOffset.UtcNow.AddSeconds(600));

                string reformedAuthenticationToken =
                   string.Format("{0}{1}{2}{3}{4}",
                   _applicationId,
                   uri,
                   method,
                   authenticationParameters[2],
                   authenticationParameters[3]);

                var secretKeyBytes = Convert.FromBase64String(_secretKey);
                var authenticationTokenBytes =
                   Encoding.UTF8.GetBytes(reformedAuthenticationToken);

                using (HMACSHA512 hmac = new HMACSHA512(secretKeyBytes))
                {
                    var hashedBytes = hmac.ComputeHash(authenticationTokenBytes);
                    string reformedTokenBase64String =
                       Convert.ToBase64String(hashedBytes);

                    if (!authenticationParameters[1].Equals(reformedTokenBase64String,
                          StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}