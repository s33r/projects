// Copyright (C) 2021 Aaron C. Willows (aaron@aaronwillows.com)
// 
// This program is free software; you can redistribute it and/or modify it under the terms of the
// GNU Lesser General Public License as published by the Free Software Foundation; either version
// 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
// the GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
// MA 02111-1307 USA

using System.Collections.Generic;
using Aaron.Core.CommandLine.Syntax;
using Aaron.Core.CommandLine.Tokens;

namespace Aaron.Core.CommandLine
{
    public delegate void CommandAction(ParsedCommandLine commandLine);

    public class ArgumentParser
    {
        private CommandBuilder _commands;
        private ParameterBuilder _defaultBuilder;

        public CommandBuilder Commands
        {
            get
            {
                if (_commands == null) { _commands = new CommandBuilder(); }

                return _commands;
            }
        }

        public CommandAction DefaultAction { get; set; }

        public ParameterBuilder DefaultBuilder
        {
            get
            {
                if (_defaultBuilder == null) { _defaultBuilder = new ParameterBuilder(); }

                return _defaultBuilder;
            }
        }

        public CommandAction ErrorAction { get; set; }
        public CommandAction FatalErrorAction { get; set; }

        public ParsedCommandLine Parse(string[] args)
        {
            List<CommandLineError> errors = new List<CommandLineError>();

            List<IToken> tokens = Tokenizer.Parse(args, errors);
            bool fatalError = errors.Find(e => e.Fatal) != null;

            if (fatalError)
            {
                ParsedCommandLine emptyCommandLine = new ParsedCommandLine();
                emptyCommandLine.Errors.AddRange(errors);
                return emptyCommandLine;
            }

            CommandBuilder instancedCommands = new CommandBuilder(_commands);
            ParameterBuilder instancedParameters = new ParameterBuilder(_defaultBuilder);

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

            if (commandLine.HasErrors) { ErrorAction?.Invoke(commandLine); }

            if (commandLine.HasFatalErrors) { FatalErrorAction?.Invoke(commandLine); }

            runAction?.Invoke(commandLine);

            return commandLine;
        }

        public ParsedCommandLine Run(string[] args)
        {
            return Run(Parse(args));
        }
    }
}