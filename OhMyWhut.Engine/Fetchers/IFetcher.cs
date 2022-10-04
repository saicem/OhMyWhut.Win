using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OhMyWhut.Engine.Data;

namespace OhMyWhut.Engine.Fetchers
{
    internal interface IFetcher
    {
        Task<FetchResult> FetchAsync();
    }
}
