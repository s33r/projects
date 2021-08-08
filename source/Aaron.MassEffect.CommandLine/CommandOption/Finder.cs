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
using Aaron.MassEffect.Coalesced;
using Aaron.MassEffect.Coalesced.Records;

namespace Aaron.MassEffect.CommandLine.CommandOption
{
    public static class Finder
    {
        public const char WILDCARD = '*';

        public static IEnumerable<IRecord> FindRecords(SearchCriteria criteria, Container container)
        {
            if (criteria is null) { throw new ArgumentNullException(nameof(criteria)); }

            if (container is null) { throw new ArgumentNullException(nameof(container)); }

            if (criteria.IsEmptySearch)
            {
                foreach (FileRecord fileRecord in container.Files) { yield return fileRecord; }
            }
            else
            {
                if (criteria.Mode == SearchMode.Regex) { criteria.CompileRegex(); }

                foreach (FileRecord fileRecord in container.Files)
                {
                    if (Match(criteria, fileRecord)) { yield return fileRecord; }

                    foreach (SectionRecord sectionRecord in fileRecord)
                    {
                        if (Match(criteria, sectionRecord)) { yield return sectionRecord; }


                        foreach (EntryRecord entryRecord in sectionRecord)
                        {
                            if (Match(criteria, entryRecord)) { yield return entryRecord; }
                        }
                    }
                }
            }
        }

        public static bool SimpleMatch(string pattern, string value)
        {
            if (string.IsNullOrEmpty(pattern)) { return false; }

            if (string.IsNullOrEmpty(value) && pattern == WILDCARD.ToString()) { return true; }

            if (string.IsNullOrEmpty(value)) { return false; }

            if (pattern == WILDCARD.ToString()) { return true; }

            string fixedPattern = pattern.ToUpperInvariant();
            string fixedValue = value.ToUpperInvariant();

            string[] pArray = fixedPattern.Split(WILDCARD, StringSplitOptions.RemoveEmptyEntries);
            Queue<string> matchers = new Queue<string>(pArray);
            string currentValue = fixedValue;

            while (matchers.Count > 0)
            {
                string currentMatcher = matchers.Dequeue();
                int offset = currentValue.IndexOf(currentMatcher, StringComparison.InvariantCultureIgnoreCase);

                if (offset >= 0) { currentValue = currentValue.Substring(offset + currentMatcher.Length); }
                else { return false; }
            }

            return true;
        }


        private static bool Match(SearchCriteria criteria, FileRecord record)
        {
            if (criteria.FileName is null) { return false; }

            if (criteria.Mode == SearchMode.Simple) { return SimpleMatch(criteria.FileName, record.Name); }

            if (criteria.Mode == SearchMode.Regex) { criteria.FileNameRegex.IsMatch(record.Name); }

            return criteria.FileName.ToUpperInvariant() == record.Name.ToUpperInvariant();
        }

        private static bool Match(SearchCriteria criteria, SectionRecord record)
        {
            if (criteria.SectionName is null) { return false; }

            if (criteria.Mode == SearchMode.Simple) { return SimpleMatch(criteria.SectionName, record.Name); }

            if (criteria.Mode == SearchMode.Regex) { criteria.SectionNameRegex.IsMatch(record.Name); }

            return criteria.SectionName.ToUpperInvariant() == record.Name.ToUpperInvariant();
        }

        private static bool Match(SearchCriteria criteria, EntryRecord record)
        {
            if (criteria.EntryName is null) { return false; }

            if (criteria.Mode == SearchMode.Simple) { return SimpleMatch(criteria.EntryName, record.Name); }

            if (criteria.Mode == SearchMode.Regex) { criteria.EntryNameRegex.IsMatch(record.Name); }

            return criteria.EntryName.ToUpperInvariant() == record.Name.ToUpperInvariant();
        }
    }
}