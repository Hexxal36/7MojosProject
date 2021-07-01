using PublicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public interface ISourceService
    {
        public Source[] GetSources(int preferred);
    }
}
