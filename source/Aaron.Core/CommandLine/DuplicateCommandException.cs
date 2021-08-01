using System;

namespace Aaron.Core.CommandLine
{
    public class DuplicateCommandException : Exception
    {
        public DuplicateCommandException(string commandName)
            : base($"The command {commandName} already exists!") { }
    }
}