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
using System.IO;
using System.Text;

namespace Aaron.Core.TextFiles.Csv
{
    public static class Parser
    {
        public static ParseResult Parse(string fileLocation, CsvOptions options)
        {
            if (fileLocation == null) { throw new ArgumentNullException(nameof(fileLocation)); }

            if (options == null) { throw new ArgumentNullException(nameof(options)); }

            using StreamReader input = new StreamReader(File.OpenRead(fileLocation), Encoding.UTF8);

            return Parse(input, options);
        }

        public static ParseResult Parse(string fileLocation)
        {
            return Parse(fileLocation, new CsvOptions());
        }

        public static ParseResult Parse(StreamReader input)
        {
            return Parse(input, new CsvOptions());
        }

        public static ParseResult Parse(StreamReader input, CsvOptions options)
        {
            if (input == null) { throw new ArgumentNullException(nameof(input)); }

            if (options == null) { throw new ArgumentNullException(nameof(options)); }

            ParseResult result = new ParseResult(options);

            bool firstLine = true;

            while (!input.EndOfStream)
            {
                string line = input.ReadLine();

                if (line.StartsWith(options.CommentCharacter)) { continue; }

                if (firstLine && options.HasHeaders) { result.Headers = ParseLine(line, options); }
                else { result.Entries.Add(ParseLine(line, options)); }

                firstLine = false;
            }

            return result;
        }


        private static List<string> ParseLine(string line, CsvOptions options)
        {
            List<string> result = new List<string>();

            bool inQuote = false;
            StringBuilder builder = new StringBuilder();
            builder.EnsureCapacity(line.Length);

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuote)
                {
                    if (c == options.Quote) { inQuote = false; }
                    else { builder.Append(c); }
                }
                else
                {
                    if (c == options.Quote) { inQuote = true; }
                    else if (c == options.Delimiter)
                    {
                        result.Add(builder.ToString());
                        builder.Clear();
                    }
                    else { builder.Append(c); }
                }
            }

            result.Add(builder.ToString());

            return result;
        }
    }
}