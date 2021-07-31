using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Core.CommandLine
{
    public class CommandLineError
    {
        public string CommandName { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
        public int ArgumentPosition { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool Fatal { get; set; }

    }
}
