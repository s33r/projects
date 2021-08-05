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
            if (supportedParameters == null) { throw new ArgumentNullException(nameof(supportedParameters)); }

            Name = name;

            ShortDescription = string.Empty;
            LongDescription = string.Empty;

            foreach (Parameter parameter in supportedParameters) { _ = Parameters.AddParameter(parameter); }
        }

        public Command(string name)
        {
            Name = name;

            ShortDescription = string.Empty;
            LongDescription = string.Empty;
        }

        public Command(Command otherCommand)
        {
            if (otherCommand == null) { throw new ArgumentNullException(nameof(otherCommand)); }

            Name = otherCommand.Name;
            ShortDescription = otherCommand.ShortDescription;
            LongDescription = otherCommand.LongDescription;
            OnExecute = otherCommand.OnExecute;

            foreach (Parameter parameter in otherCommand.Parameters.ToList())
            {
                _ = Parameters.AddParameter(parameter);
            }
        }

        public override string ToString()
        {
            return $"Command: {Name} - {ShortDescription}";
        }
    }
}