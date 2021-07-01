using PublicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    public class SourceService : ISourceService
    {
        public Source[] GetSources(int preferred)
        {
            var sources = new Source[]
            {
                new Source()
                {
                    Url = "https://localhost:44316/today",
                    Priority = 2,
                },
                new Source()
                {
                    Url = "https://localhost:44396/today",
                    Priority = 2,
                },
            };

            if (preferred < sources.Length)
            {
                sources[preferred].Priority = 1;
            }

            return sources;
        }
    }
}
