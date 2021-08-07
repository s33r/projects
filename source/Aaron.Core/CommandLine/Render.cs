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
using System.Text;
using Aaron.Core.Extensions;

namespace Aaron.Core.CommandLine
{
    public static class Render
    {
        public const int MAX_WIDTH = 100;

        public static IEnumerable<string> Banner(string title, string copyright, string message)
        {
            string topBorder = $"╔{new string('═', MAX_WIDTH - 2)}╗";
            string midBorder = $"╟{new string('─', MAX_WIDTH - 2)}╢";
            string bottomBorder = $"╚{new string('═', MAX_WIDTH - 2)}╝";

            int titleLength = Emoji.GetActualLength(title);
            int copyrightLength = Emoji.GetActualLength(copyright);
            int messageLength = Emoji.GetActualLength(message);

            List<string> result = new List<string>
            {
                topBorder,
                $"║ {title.Center(MAX_WIDTH - 4, titleLength, ".")} ║",
                $"║ {copyright.Center(MAX_WIDTH - 4, copyrightLength, ".")} ║",
                midBorder,
                $"║ {message.PadLeft(MAX_WIDTH - 4, messageLength, " ")} ║",
                bottomBorder,
            };

            return result;
        }

        public static IEnumerable<string> ColumnHeaders()
        {
            const int width = MAX_WIDTH / 10;

            StringBuilder line1 = new StringBuilder();
            StringBuilder line2 = new StringBuilder();
            for (int i = 0; i < width; i++)
            {
                line1.Append(i.ToString(CultureInfo.InvariantCulture).PadRight(10, '-'));
                line2.Append("0123456789");
            }

            return new List<string>
            {
                line1.ToString(),
                line2.ToString(),
            };
        }

        public static void InitializeConsole()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.OutputEncoding = Encoding.UTF8;
        }


        public static void Write(IEnumerable<string> lines)
        {
            if (lines is null) { throw new ArgumentNullException(nameof(lines)); }

            foreach (string line in lines) { Console.WriteLine(line); }
        }
    }
}