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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aaron.MassEffect.Coalesced.Records;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced
{
    public class Container : IEnumerable<IRecord>
    {
        public List<FileRecord> Files { get; }

        public IEnumerable<EntryRecord> GetEntries
        {
            get
            {
                foreach (FileRecord fileRecord in Files)
                {
                    foreach (SectionRecord sectionRecord in fileRecord)
                    {
                        foreach (EntryRecord entryRecord in sectionRecord) { yield return entryRecord; }
                    }
                }
            }
        }

        public IEnumerable<string> GetItems
        {
            get
            {
                foreach (FileRecord fileRecord in Files)
                {
                    foreach (SectionRecord sectionRecord in fileRecord)
                    {
                        foreach (EntryRecord entryRecord in sectionRecord)
                        {
                            foreach (string item in entryRecord) { yield return item; }
                        }
                    }
                }
            }
        }

        public IEnumerable<SectionRecord> GetSections
        {
            get
            {
                foreach (FileRecord fileRecord in Files)
                {
                    foreach (SectionRecord sectionRecord in fileRecord) { yield return sectionRecord; }
                }
            }
        }

        public int RecordCount => GetEntries.Count();

        public Container()
        {
            Files = new List<FileRecord>();
        }

        public Container(int count)
        {
            Files = Utility.CreateList<FileRecord>(count).ToList();
        }

        public Container(IEnumerable<FileRecord> records)
        {
            Files = new List<FileRecord>(records);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IRecord> GetEnumerator()
        {
            foreach (FileRecord fileRecord in Files)
            {
                yield return fileRecord;

                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    yield return sectionRecord;

                    foreach (EntryRecord entryRecord in sectionRecord) { yield return entryRecord; }
                }
            }
        }

        public string DumpRecords()
        {
            StringBuilder output = new StringBuilder();

            foreach (FileRecord fileRecord in Files)
            {
                _ = output.AppendLine($"{fileRecord.Name}");

                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    _ = output.AppendLine($"    {sectionRecord.Name}");

                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        _ = output.AppendLine($"        {entryRecord.Name}");

                        int valueIndex = 0;
                        foreach (string value in entryRecord)
                        {
                            _ = output.AppendLine($"            [{valueIndex++}] {value}");
                        }
                    }
                }
            }

            return output.ToString();
        }

        public void DumpRecords(string fileName)
        {
            string outputLocation = Path.Join(MassEffectConfiguration.Instance.WorkingLocation, fileName);

            string result = DumpRecords();

            File.WriteAllText(outputLocation, result);
        }

        public string GetData()
        {
            return GetData(out _);
        }

        public string GetData(out int longestLength)
        {
            int longest = 0;
            StringBuilder dataBuffer = new StringBuilder();

            foreach (string item in GetItems)
            {
                _ = dataBuffer.Append(item + '\0');

                if (item.Length > longest) { longest = item.Length; }
            }

            longestLength = longest;
            return dataBuffer.ToString();
        }


        public void Sort(Comparison<IRecord> comparison)
        {
            Files.Sort(comparison);

            foreach (FileRecord fileRecord in Files)
            {
                fileRecord.Sort(comparison);

                foreach (SectionRecord sectionRecord in fileRecord) { sectionRecord.Sort(comparison); }
            }
        }
    }
}