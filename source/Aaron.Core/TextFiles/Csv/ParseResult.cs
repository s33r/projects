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
using System.Linq;

namespace Aaron.Core.TextFiles.Csv
{
    public class ParseResult
    {
        public List<List<string>> Entries { get; set; } = new List<List<string>>();
        public List<string> Headers { get; set; } = new List<string>();

        public CsvOptions Options { get; }

        public ParseResult(CsvOptions options)
        {
            Options = options;
        }


        public List<Dictionary<string, string>> ToDictionary()
        {
            return Entries.Select(row => MapRow(row)).ToList();
        }


        private Dictionary<string, string> MapRow(List<string> row)
        {
            List<string> headers = Headers.Select(h => h.ToUpperInvariant().Trim()).ToList();

            Dictionary<string, string> result = new Dictionary<string, string>(row.Count);

            for (int i = 0; i < row.Count; i++) { result.Add(headers[i], row[i]); }

            return result;
        }
    }
}