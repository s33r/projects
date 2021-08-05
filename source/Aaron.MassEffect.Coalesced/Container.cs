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
    public class Container : IEnumerable<IRecordCollection>
    {
        public List<FileRecordCollection> Files { get; }

        public IEnumerable<EntryRecordCollection> GetEntries
        {
            get
            {
                foreach (FileRecordCollection fileRecord in Files)
                {
                    foreach (SectionRecordCollection sectionRecord in fileRecord)
                    {
                        foreach (EntryRecordCollection entryRecord in sectionRecord) { yield return entryRecord; }
                    }
                }
            }
        }

        public IEnumerable<string> GetItems
        {
            get
            {
                foreach (FileRecordCollection fileRecord in Files)
                {
                    foreach (SectionRecordCollection sectionRecord in fileRecord)
                    {
                        foreach (EntryRecordCollection entryRecord in sectionRecord)
                        {
                            foreach (string item in entryRecord) { yield return item; }
                        }
                    }
                }
            }
        }

        public IEnumerable<SectionRecordCollection> GetSections
        {
            get
            {
                foreach (FileRecordCollection fileRecord in Files)
                {
                    foreach (SectionRecordCollection sectionRecord in fileRecord) { yield return sectionRecord; }
                }
            }
        }

        public int RecordCount => GetEntries.Count();

        public Container()
        {
            Files = new List<FileRecordCollection>();
        }

        public Container(int count)
        {
            Files = Utility.CreateList<FileRecordCollection>(count).ToList();
        }

        public Container(IEnumerable<FileRecordCollection> records)
        {
            Files = new List<FileRecordCollection>(records);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IRecordCollection> GetEnumerator()
        {
            foreach (FileRecordCollection fileRecord in Files)
            {
                yield return fileRecord;

                foreach (SectionRecordCollection sectionRecord in fileRecord)
                {
                    yield return sectionRecord;

                    foreach (EntryRecordCollection entryRecord in sectionRecord) { yield return entryRecord; }
                }
            }
        }

        public string DumpRecords()
        {
            StringBuilder output = new StringBuilder();

            foreach (FileRecordCollection fileRecord in Files)
            {
                _ = output.AppendLine($"{fileRecord.Name}");

                foreach (SectionRecordCollection sectionRecord in fileRecord)
                {
                    _ = output.AppendLine($"    {sectionRecord.Name}");

                    foreach (EntryRecordCollection entryRecord in sectionRecord)
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


        public void Sort(Comparison<IRecordCollection> comparison)
        {
            Files.Sort(comparison);

            foreach (FileRecordCollection fileRecord in Files)
            {
                fileRecord.Sort(comparison);

                foreach (SectionRecordCollection sectionRecord in fileRecord) { sectionRecord.Sort(comparison); }
            }
        }
    }
}