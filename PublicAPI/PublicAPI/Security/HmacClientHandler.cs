using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace PublicAPI.Security
{
    internal class HmacClientHandler : DelegatingHandler
    {

        private string _applicationId = "XYZ-123";
        private string _secretKey = "MYSECRETKEY1";

        protected async override Task<HttpResponseMessage>
           SendAsync(HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            string url = Uri.EscapeUriString(request.
               RequestUri.ToString().ToLowerInvariant());
            string methodName = request.Method.Method;

            string timeStamp = DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss");
            string nonce = Guid.NewGuid().ToString();

            //Formulate the keys used in plain format as
            //a concatenated string.
            //Note that the secret key is not provided here.
            string authenticationKeyString =
               string.Format("{0}{1}{2}{3}{4}",
               _applicationId, url, methodName, nonce, timeStamp);

            var secretKeyBase64ByteArray =
               Convert.FromBase64String(_secretKey);

            using (HMAC hmac = new HMACSHA512(secretKeyBase64ByteArray))
            {
                byte[] authenticationKeyBytes = Encoding.UTF8.GetBytes(authenticationKeyString);
                byte[] authenticationHash = hmac.ComputeHash(authenticationKeyBytes);
                string hashedBase64String = Convert.ToBase64String(authenticationHash);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                   "hmac",
                   $"{_applicationId}:{hashedBase64String}:{nonce}:{timeStamp}");
            }

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}