using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Automation
{
    public class InvalidScriptException : Exception
    {
        public InvalidScriptException(string message) : base(message)
        {
        }

        public InvalidScriptException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidScriptException()
        {
        }
    }
}
