using System.Collections.Generic;

namespace Aaron.Core.CommandLine.Syntax
{
    public class ParsedCommandLine
    {
        public Command Command { get; set; }
        public List<Parameter> Parameters { get; set; }
        public string Leftover { get; set; }

        public List<CommandLineError> Errors { get; } = new();

        public bool HasFatalErrors => Errors.Find(e => e.Fatal) != null;

        public bool HasErrors => Errors.Count > 0;
    }
}
