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
using System.Linq;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Automation.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (string arg in args) { Console.WriteLine(arg); }

            Console.WriteLine();

            Render.InitializeConsole();
            Render.Write(Render.Banner(
                $"{Emoji.Gear} Repo Automation {Emoji.Gear}",
                "Copyright ©️ 2021 - Aaron C. Willows",
                "Alpha"
            ));

            ArgumentParser parser = new ArgumentParser();
            new CommandFactory(parser.Commands)
                .AddInitCommand();

#if DEBUG
            string[] debugArgs = { "init", "-t", "web", "-n", "init test" };
            ParsedCommandLine commandLine = parser.Parse(debugArgs);
#else
            ParsedCommandLine commandLine = parser.Parse(args);
#endif
            if (commandLine.HasErrors) { Render.Write(commandLine.Errors.Select(error => error.ToString())); }

            if (commandLine.HasFatalErrors) { return; }

            parser.Run(commandLine);
        }
    }
}