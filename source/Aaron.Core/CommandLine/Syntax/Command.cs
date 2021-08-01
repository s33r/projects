using System.Collections.Generic;

namespace Aaron.Core.CommandLine.Syntax
{
    public class Command
    {
        public string LongDescription { get; set; }
        public string Name { get; set; }
        public CommandAction OnExecute { get; set; }
        public ParameterBuilder Parameters { get; } = new ParameterBuilder();
        public string ShortDescription { get; set; }

        public Command() { }

        public Command(string name, IEnumerable<Parameter> supportedParameters)
        {
            Name = name;

            ShortDescription = string.Empty;
            LongDescription = string.Empty;

            foreach (Parameter parameter in supportedParameters) { Parameters.AddParameter(parameter); }
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

            foreach (Parameter parameter in otherCommand.Parameters.ToList()) { Parameters.AddParameter(parameter); }
        }

        public override string ToString()
        {
            return $"Command: {Name} - {ShortDescription}";
        }
    }
}