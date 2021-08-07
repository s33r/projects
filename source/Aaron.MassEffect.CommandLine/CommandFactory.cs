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

using System;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;
using Aaron.MassEffect.CommandLine.CommandConfig;

namespace Aaron.MassEffect.CommandLine
{
    internal class CommandFactory
    {
        public CommandBuilder Builder { get; }

        public CommandFactory(CommandBuilder builder)
        {
            Builder = builder;
        }

        public CommandFactory AddConfigCommand()
        {
            Command command = new Command
            {
                Name = "config",
                LongDescription = "Do things related to save files",
                ShortDescription = "Do things related to save files",
                OnExecute = Runner.Execute,
            };


            Builder.AddCommand(command);


            return this;
        }

        public CommandFactory AddOptionCommand()
        {
            Command command = new Command
            {
                Name = "option",
                LongDescription = "Do things released to Coalesced.bin files",
                ShortDescription = "Do things released to Coalesced.bin files",
                OnExecute = CommandOption.Runner.Execute,
            };

            command.Parameters.AddParameter(new Parameter
            {
                Name = "get",
                ShortDescription = "Gets one or more values from the coalesced.bin file",
                LongDescription = "Gets one or more values from the coalesced.bin file",
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "set",
                ShortDescription = "Sets a value in the coalesced.bin file",
                LongDescription = "Sets a value in the coalesced.bin file",
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "file",
                Alias = "f",
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "section",
                Alias = "s",
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "entry",
                Alias = "e",
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "index",
                Alias = "i",
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "type",
                Alias = "t",
                DefaultValue = "fancy",
                Options =
                {
                    "literal",
                    "simple",
                    "regex",
                },
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "game",
                Alias = "g",
                DefaultValue = "me1",
                Options =
                {
                    "me1",
                    "me2",
                    "me3",
                },
            });

            command.Parameters.AddParameter(new Parameter
            {
                Name = "path",
                Alias = "p",
            });


            command.Parameters.AddParameter(new Parameter
            {
                Name = "display",
                Alias = "d",
                DefaultValue = "fancy",
                Options =
                {
                    "json",
                    "csv",
                    "fancy",
                },
            });

            Builder.AddCommand(command);

            return this;
        }

        public CommandFactory AddSaveCommand()
        {
            Builder.AddCommand(new Command
            {
                Name = "save",
                LongDescription = "Configures me.exe",
                ShortDescription = "Configures me.exe",
                OnExecute = pcl => Console.WriteLine(pcl.Command),
            });

            return this;
        }
    }
}