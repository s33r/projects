using System;
using System.Collections.Generic;

namespace Aaron.Core.CommandLine.Syntax
{
    public class Command
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public ParameterBuilder Parameters { get; } = new();
        public CommandAction OnExecute { get; set; }

        public Command() { }

        public Command(string name, IEnumerable<Parameter> supportedParameters)
        {
            Name = name;

            ShortDescription = string.Empty;
            LongDescription = string.Empty;

            foreach (Parameter parameter in supportedParameters)
            {
                Parameters.AddParameter(parameter);
            }
        }

        public Command(string name)
        {
            Name = name;
            
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
        }

        public Command(Command otherCommand)
        {
            Name = otherCommand.Name;
            ShortDescription = otherCommand.ShortDescription;
            LongDescription = otherCommand.LongDescription;
            OnExecute = otherCommand.OnExecute;

            foreach (Parameter parameter in otherCommand.Parameters.ToList())
            {
                Parameters.AddParameter(parameter);
            }
        }
    }
}
