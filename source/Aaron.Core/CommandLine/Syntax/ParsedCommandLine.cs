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

namespace Aaron.Core.CommandLine.Syntax
{
    public class ParsedCommandLine
    {
        public Command Command { get; set; }

        public List<CommandLineError> Errors { get; } = new List<CommandLineError>();

        public bool HasErrors => Errors.Count > 0;

        public bool HasFatalErrors => Errors.Find(e => e.Fatal) != null;
        public string Leftover { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public List<Parameter> Parameters { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}