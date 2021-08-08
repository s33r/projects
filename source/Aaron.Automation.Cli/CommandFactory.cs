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

using Aaron.Automation.Cli.CommandInit;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Automation.Cli
{
    public class CommandFactory
    {
        public CommandBuilder Builder { get; }

        public CommandFactory(CommandBuilder builder)
        {
            Builder = builder;
        }

        public CommandFactory AddInitCommand()
        {
            Runner runner = new Runner();

            Command command = new Command
            {
                Name = "init",
                LongDescription = "Creates a new project",
                ShortDescription = "Creates a new project",
                OnExecute = runner.Execute,
            };

            command.Parameters.AddParameter(new Parameter
            {
                Required = true,
                Name = "type",
                Alias = "t",
                ShortDescription = "The type of project to create",
                LongDescription = "The type of project to create. Either a command passed to dotnet new or \"node\"",
            });

            command.Parameters.AddParameter(new Parameter
            {
                Required = true,
                Name = "name",
                Alias = "n",
                ShortDescription = "The name of the proejct to create",
                LongDescription = "The name of the proejct to create",
            });

            Builder.AddCommand(command);
            return this;
        }
    }
}