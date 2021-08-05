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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aaron.MassEffect.Coalesced.Records;
using Aaron.MassEffect.Core;
using Encoder = Aaron.Binary.Compression.Huffman.Encoder;

namespace Aaron.MassEffect.Coalesced.Me3.DataStructures
{
    internal class IndexContainer
    {
        public StandardIndex Index { get; set; }

        public Section[] Sections { get; set; }

        public IndexContainer() { }

        public IndexContainer(ushort count)
        {
            Index = new StandardIndex(count);
            Sections = new Section[count];

            for (int i = 0; i < count; i++) { Sections[i] = new Section(); }
        }

        public string Dump()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine($"Expected Size = {TotalSize()}");

            foreach (StandardIndexEntry fileIndexEntry in Index.Table)
            {
                output.AppendLine($"{fileIndexEntry.Offset,10} | {fileIndexEntry.StringTableIndex}");
            }

            output.AppendLine();

            foreach (Section section in Sections)
            {
                output.AppendLine($"Section ({section.Index.Count} entries / {section.Size()} bytes)");
                foreach (Entry entry in section.Entries)
                {
                    output.AppendLine($"    Entry ({entry.Index.Count} entries / {entry.Size()} bytes)");
                    foreach (Item item in entry.Items)
                    {
                        output.AppendLine($"        Item ({item.Count} entries / {item.Size()} bytes)");
                        foreach (int value in item.Values)
                        {
                            //output.AppendLine(value.ToString());
                        }
                        //output.AppendLine();
                    }
                }
            }

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.index.txt";
            string outputLocation = Path.Join(MassEffectConfiguration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }

        public static IndexContainer FromRecords(Container container, StringTableBlock stringTable,
                                                 Encoder encoder, BitArray compressedData)
        {
            container.Sort((x, y) => SortByIndexComparer(x, y, stringTable));

            IndexContainer indexContainer = new IndexContainer((ushort)container.Files.Count);

            int bitOffset = 0;
            uint fileOffset = (ushort)indexContainer.Size();

            for (int sectionIndex = 0; sectionIndex < indexContainer.Sections.Length; sectionIndex++)
            {
                FileRecordCollection currentFileRecordCollection = container.Files[sectionIndex];
                Section currentSection =
                    new Section((ushort)currentFileRecordCollection.Count, indexContainer.Index.Table[sectionIndex]);
                indexContainer.Sections[sectionIndex] = currentSection;

                currentSection.Parent.Offset = fileOffset;
                currentSection.Parent.StringTableIndex = stringTable.IndexOf(currentFileRecordCollection.Name);

                uint sectionOffset = currentSection.Size();

                for (int entryIndex = 0; entryIndex < currentSection.Entries.Length; entryIndex++)
                {
                    SectionRecordCollection currentSectionRecordCollection = container.Files[sectionIndex][entryIndex];
                    Entry currentEntry =
                        new Entry((ushort)currentSectionRecordCollection.Count, currentSection.Index.Table[entryIndex]);
                    currentSection.Entries[entryIndex] = currentEntry;

                    currentEntry.Parent.Offset = sectionOffset;
                    currentEntry.Parent.StringTableIndex = stringTable.IndexOf(currentSectionRecordCollection.Name);

                    ushort entryOffset = currentEntry.Size();

                    for (int itemIndex = 0; itemIndex < currentEntry.Items.Length; itemIndex++)
                    {
                        EntryRecordCollection currentEntryRecordCollection =
                            container.Files[sectionIndex][entryIndex][itemIndex];
                        Item currentItem =
                            new Item((ushort)currentSectionRecordCollection[itemIndex].Count,
                                currentEntry.Index.Table[itemIndex]);
                        currentEntry.Items[itemIndex] = currentItem;

                        currentItem.Parent.Offset = entryOffset;
                        currentItem.Parent.StringTableIndex = stringTable.IndexOf(currentEntryRecordCollection.Name);

                        for (int valueIndex = 0; valueIndex < currentItem.Count; valueIndex++)
                        {
                            string currentValue = currentEntryRecordCollection[valueIndex];
                            bitOffset = currentItem.Encode(currentValue, valueIndex, bitOffset, encoder,
                                compressedData);
                        }

                        entryOffset += currentItem.Size();
                    }

                    sectionOffset += entryOffset;
                }

                fileOffset += sectionOffset;
            }


            return indexContainer;
        }

        public void Read(BinaryReader reader)
        {
            Index = new StandardIndex();
            Index.Read(reader);

            Sections = new Section[Index.Count];

            for (int i = 0; i < Index.Count; i++)
            {
                reader.BaseStream.Seek(Index[i].Offset, SeekOrigin.Begin);

                Sections[i] = new Section();
                Sections[i].Read(reader, Index[i].Offset, Index[i]);
            }
        }

        public int Size()
        {
            return Index.Size();
        }

        public static int Size(int count)
        {
            return StandardIndex.Size(count);
        }

        public Container ToRecords(StringTableBlock stringTable, HuffmanTreeBlock huffmanTree, BitArray compressedData,
                                   int maxValueLength)
        {
            List<FileRecordCollection> fileRecords = Index.Table
                                                          .Select(f =>
                                                              new FileRecordCollection(f.GetString(stringTable)))
                                                          .ToList();

            for (int sectionIndex = 0; sectionIndex < Sections.Length; sectionIndex++)
            {
                Section currentSection = Sections[sectionIndex];

                fileRecords[sectionIndex]
                    .SetValues(currentSection.Index.Table
                                             .Select(s => new SectionRecordCollection(s.GetString(stringTable)))
                                             .ToList());

                for (int entryIndex = 0; entryIndex < currentSection.Entries.Length; entryIndex++)
                {
                    Entry currentEntry = currentSection.Entries[entryIndex];

                    fileRecords[sectionIndex][entryIndex]
                        .SetValues(currentEntry.Index.Table
                                               .Select(e => new EntryRecordCollection(e.GetString(stringTable)))
                                               .ToList());

                    for (int itemIndex = 0; itemIndex < currentEntry.Items.Length; itemIndex++)
                    {
                        Item currentItem = currentEntry.Items[itemIndex];

                        fileRecords[sectionIndex][entryIndex][itemIndex]
                            .SetValues(currentItem.Decode(huffmanTree, compressedData, maxValueLength));
                    }
                }
            }

            Container container = new Container(fileRecords);
            //container.Sort();

            return container;
        }

        public int TotalSize()
        {
            int totalSize = Size();

            foreach (Section sectionIndex in Sections)
            {
                totalSize += sectionIndex.Size();

                foreach (Entry entryIndex in sectionIndex.Entries)
                {
                    totalSize += entryIndex.Size();

                    foreach (Item itemIndex in entryIndex.Items) { totalSize += itemIndex.Size(); }
                }
            }

            return totalSize;
        }

        public void Write(BinaryWriter writer)
        {
            Index.Write(writer);

            foreach (Section sectionIndex in Sections) { sectionIndex.Write(writer); }
        }

        private static int SortByIndexComparer(IRecordCollection x, IRecordCollection y, StringTableBlock stringTable)
        {
            return stringTable.IndexOf(x.Name).CompareTo(stringTable.IndexOf(y.Name));
        }
    }
}