using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Engine.Exceptions
{
    public class RequestFailedException : Exception
    {
        public RequestFailedException() : base()
        {

        }

        public RequestFailedException(string message) : base(message)
        {

        }
    }
}
