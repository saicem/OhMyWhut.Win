using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Engine.Data
{
    public enum FetchStatus
    {
        Success,
        CanNotReach,
        MissLoginStatus,
    }

    public class FetchResult
    {
        public FetchResult(FetchStatus status, string data)
        {
            Status = status;
            Data = data;
        }

        public FetchStatus Status { get; }

        public string Data { get; }
    }
}
