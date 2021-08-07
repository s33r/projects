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
using System.Text.RegularExpressions;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.CommandLine.CommandOption
{
    public enum SearchMode
    {
        Literal,
        Simple,
        Regex,
    }

    public class SearchCriteria
    {
        public const RegexOptions REGEX_OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;

        public string EntryName { get; set; }

        public Regex EntryNameRegex { get; private set; }

        public string FileName { get; set; }
        public Regex FileNameRegex { get; private set; }
        public Games Game { get; set; }
        public string Index { get; set; }

        public bool IsEmptySearch =>
            string.IsNullOrEmpty(FileName)
            && string.IsNullOrEmpty(SectionName)
            && string.IsNullOrEmpty(EntryName);

        public SearchMode Mode { get; set; }
        public string SectionName { get; set; }
        public Regex SectionNameRegex { get; private set; }

        public SearchCriteria() { }

        public SearchCriteria(string path)
        {
            if (string.IsNullOrEmpty(path)) { return; }

            string[] parts = path.Split('/', StringSplitOptions.TrimEntries);

            if (parts.Length >= 1) { FileName = parts[0]; }

            if (parts.Length >= 2) { SectionName = parts[1]; }

            if (parts.Length >= 3) { EntryName = parts[2]; }

            if (parts.Length >= 4) { Index = parts[3]; }
        }

        public void CompileRegex()
        {
            EntryNameRegex = new Regex(EntryName, REGEX_OPTIONS);
            FileNameRegex = new Regex(FileName, REGEX_OPTIONS);
            SectionNameRegex = new Regex(SectionName, REGEX_OPTIONS);
        }

        public override string ToString()
        {
            return $"{Mode} {Game} [File={FileName}][Section={SectionName}][Entry={EntryName}][Index={Index}]";
        }
    }
}