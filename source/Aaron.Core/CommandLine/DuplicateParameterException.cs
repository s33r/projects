using System;

namespace Aaron.Core.CommandLine
{
    public class DuplicateParameterException : Exception
    {
        public DuplicateParameterException(string parameterName)
            : base($"The parameter {parameterName} already exists!") { }

        public DuplicateParameterException(string parameterName, string commandName)
            : base($"The parameter {parameterName} already exists for command {commandName}!") { }
    }
}