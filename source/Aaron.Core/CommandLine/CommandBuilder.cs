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
using System.Collections.Generic;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Core.CommandLine
{
    public class CommandBuilder
    {
        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>();

        public CommandBuilder() { }

        public CommandBuilder(CommandBuilder other)
        {
            if (other == null) { return; }

            List<Command> otherCommands = other.ToList();

            foreach (Command command in otherCommands) { AddCommand(new Command(command)); }
        }

        public CommandBuilder AddCommand(Command command)
        {
            if (string.IsNullOrEmpty(command.Name))
            {
                throw new ArgumentException("The name of the command cannot be null.");
            }

            if (_commands.ContainsKey(command.Name)) { throw new DuplicateCommandException(command.Name); }

            _commands.Add(command.Name, command);

            return this;
        }

        public bool HasCommand(string name)
        {
            return _commands.ContainsKey(name);
        }

        public Dictionary<string, Command> ToDictionary()
        {
            Dictionary<string, Command> result = new Dictionary<string, Command>();

            foreach (KeyValuePair<string, Command> keyValuePair in _commands)
            {
                result.Add(keyValuePair.Key, new Command(keyValuePair.Value));
            }

            return result;
        }

        public List<Command> ToList()
        {
            return new List<Command>(_commands.Values);
        }
    }
}