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
using System.Globalization;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Automation.Cli.CommandInit
{
    internal class Runner : IRunner
    {
        public void Execute(ParsedCommandLine commandLine)
        {
            Dictionary<string, Parameter> parameters = commandLine.Command.Parameters.ToDictionary();

            string projectName = FormatName(parameters["name"].Value);
            string projectType = parameters["type"].Value;

            Execute(projectName, projectType);
        }

        public static string Execute(string projectName, string projectType)
        {
            if (string.IsNullOrEmpty(projectName)) { return $"{Emoji.Cross} The project name is invalid."; }

            if (string.IsNullOrEmpty(projectType)) { return $"{Emoji.Cross} The project type is invalid."; }

            if (projectType == "node") { return $"{Emoji.Cross} Creating node projects isn't supported yet."; }

            string projectPath = $"./source/{projectName}";

            string commandNew = $"dotnet new {projectType} -n {projectName} -o {projectPath}";
            string commandAdd = $"dotnet sln add --in-root {projectPath}";

            Console.WriteLine($"Running Command: {commandNew}");
            bool success = CommandRunner.Execute(commandNew);

            if (success)
            {
                Console.WriteLine($"Running Command: {commandAdd}");
                success &= CommandRunner.Execute(commandAdd);
            }

            if (!success)
            {
                Environment.ExitCode = 1;
                return $"{Emoji.Cross} Failed";
            }

            return $"{Emoji.StarGlow} Sucess";
        }

        public static string FormatName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return name; }

            string newName = CultureInfo.InvariantCulture.TextInfo.ToLower(name);
            newName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(newName);
            newName = newName.Replace(' ', '.');
            newName = $"Aaron.{newName}";

            return newName;
        }
    }
}