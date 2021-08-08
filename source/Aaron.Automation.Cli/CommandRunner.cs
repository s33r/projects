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

using System.Diagnostics;

namespace Aaron.Automation.Cli
{
    internal class CommandRunner
    {
        public const string POWERSHELL = "powershell";

        public static bool Execute(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(POWERSHELL, command);

            Process process = Process.Start(startInfo);

            process.WaitForExit();

            return process.ExitCode == 0;
        }
    }
}