using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceTwo.Services
{
    public interface IHmacAuthenticationService
    {
        public bool IsValidRequest(string authHeader, string authHeaderValue, string method, string uri);
    }
}
