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
using System.Diagnostics;
using System.Management.Automation;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Automation.Cli.Commands
{
    internal class Publish
    {
        public static Command GetCommand()
        {
            Command result = new Command
            {
                Name = "publish",
                ShortDescription = "Publishes everything",
                LongDescription = "Publishes everything",
                OnExecute = OnExecute,
            };

            return result;
        }

        private static void OnExecute(ParsedCommandLine commandLine)
        {
            PowerShell shell = PowerShell.Create();
            shell.Commands.AddCommand("ls");

            shell.Invoke();


            ProcessStartInfo startInfo = new ProcessStartInfo("cmd")
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                Arguments = "dotnet publish ./source -o ./output2",
            };


            Process process = Process.Start(startInfo);

            process.WaitForExit();

            Console.WriteLine($"True! {process.ExitCode}");
        }
    }
}