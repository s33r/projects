using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Core.CommandLine
{
    public class CommandBuilder
    {

        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>();

        public CommandBuilder AddCommand(Command command)
        {
            if (string.IsNullOrEmpty(command.Name))
            {
                throw new ArgumentException("The name of the command cannot be null.");
            }

            if (_commands.ContainsKey(command.Name))
            {
                throw new DuplicateCommandException(command.Name);
            }

            _commands.Add(command.Name, command);

            return this;
        }

        public bool HasCommand(string name)
        {
            return _commands.ContainsKey(name);
        }

        public List<Command> ToList()
        {
            return new (_commands.Values);
        }

        public Dictionary<string, Command> ToDictionary()
        {
            Dictionary<string, Command> result = new();

            foreach (KeyValuePair<string, Command> keyValuePair in _commands)
            {
                result.Add(keyValuePair.Key, new Command(keyValuePair.Value));
            }

            return result;
        }

        public CommandBuilder() { }

        public CommandBuilder(CommandBuilder other)
        {
            if (other == null)
            {
                return;
            }

            List<Command> otherCommands = other.ToList();

            foreach (Command command in otherCommands)
            {
                AddCommand(new Command(command));
            }
        }

    }
}
