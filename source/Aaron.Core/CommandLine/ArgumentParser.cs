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



        public ParsedCommandLine Run(string[] args)
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


            ParsedCommandLine commandLine;
            CommandAction runAction;

            CommandBuilder instancedCommands = new(_commands);
            ParameterBuilder instancedParameters = new(_defaultBuilder);

            if (_commands == null)
            {
                commandLine = Parser.Parse(tokens, instancedParameters.ToDictionary(), errors);
                runAction = DefaultAction;
            }
            else
            {
                commandLine = Parser.Parse(tokens, instancedCommands.ToDictionary(), errors);
                runAction = commandLine.Command.OnExecute;
            }


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

        


    }
}
