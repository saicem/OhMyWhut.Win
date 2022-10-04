using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Engine.Exceptions
{
    public class ParseFailedException : Exception
    {
        public ParseFailedException() : base()
        {

        }

        public ParseFailedException(string message) : base(message)
        {

        }
    }
}
