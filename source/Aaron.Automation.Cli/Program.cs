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
using Aaron.Automation.Cli.Commands;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Automation.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommandRunner runner = new CommandRunner("ls");
            runner.Execute();


            ArgumentParser parser = new ArgumentParser();

            _ = parser.Commands.AddCommand(Clean.GetCommand())
                      .AddCommand(NewProject.GetCommand())
                      .AddCommand(Publish.GetCommand());


            Run(args, parser);
        }

        private static void Run(string[] args, ArgumentParser parser)
        {
            Console.WriteLine("Build CLI");
            Console.WriteLine("args = {0}", string.Join(' ', args));

            ParsedCommandLine commandLine = parser.Run(args);

            if (commandLine.HasErrors) { Console.WriteLine($"{commandLine.Errors.Count} Errors"); }

            Console.WriteLine("-- Complete --");
        }
    }
}