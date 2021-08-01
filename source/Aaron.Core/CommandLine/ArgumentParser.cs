using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aaron.Core.CommandLine.Syntax;
using Aaron.Core.CommandLine.Tokens;

namespace Aaron.Core.CommandLine
{

    public delegate void CommandAction(ParsedCommandLine commandLine);
    public class ArgumentParser
    {
        private ParameterBuilder _defaultBuilder;
        private CommandBuilder _commands;

        public ParameterBuilder DefaultBuilder
        {
            get
            {
                if (_defaultBuilder == null)
                {
                    _defaultBuilder = new ParameterBuilder();
                }

                return _defaultBuilder;
            }
        }

        public CommandBuilder Commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = new CommandBuilder();
                }

                return _commands;
            }
        }

        public CommandAction DefaultAction { get; set; }
        public CommandAction ErrorAction { get; set; }
        public CommandAction FatalErrorAction { get; set; }

        public ParsedCommandLine Parse(string[] args)
        {
            List<CommandLineError> errors = new();

            List<IToken> tokens = Tokenizer.Parse(args, errors);
            bool fatalError = errors.Find(e => e.Fatal) != null;

            if (fatalError)
            {
                ParsedCommandLine emptyCommandLine = new();
                emptyCommandLine.Errors.AddRange(errors);
                return emptyCommandLine;
            }

            CommandBuilder instancedCommands = new(_commands);
            ParameterBuilder instancedParameters = new(_defaultBuilder);

            ParsedCommandLine commandLine = _commands == null 
                ? Parser.Parse(tokens, instancedParameters.ToDictionary(), errors) 
                : Parser.Parse(tokens, instancedCommands.ToDictionary(), errors);

            return commandLine;
        }


        public ParsedCommandLine Run(ParsedCommandLine commandLine)
        {
            CommandAction runAction = _commands == null
                ? DefaultAction
                : commandLine.Command.OnExecute;

            if (commandLine.HasErrors)
            {
                ErrorAction?.Invoke(commandLine);
            }

            if (commandLine.HasFatalErrors)
            {
                FatalErrorAction?.Invoke(commandLine);
            }

            runAction?.Invoke(commandLine);

            return commandLine;
        }

        public ParsedCommandLine Run(string[] args)
        {
            return Run(Parse(args));
        }

        


    }
}
