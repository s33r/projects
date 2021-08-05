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
using System.Linq;
using System.Text;
using Aaron.MassEffect.Coalesced.Me3.DataStructures;
using Aaron.MassEffect.Coalesced.Records;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced.Me3
{
    internal class StringTableBlock : IBlock<Codec>
    {
        public const int ENTRY_COUNT_LENGTH = 4;
        public const int HEADER_LENGTH = HEADER_TOTAL_SIZE_LENGTH + ENTRY_COUNT_LENGTH;
        public const int HEADER_TOTAL_SIZE_LENGTH = 4;
        public const int INDEX_ENTRY_LENGTH = 8;

        public List<StringTableEntry> Entries { get; private set; }

        public string Dump()
        {
            StringBuilder output = new StringBuilder();

            foreach (StringTableEntry entry in Entries)
            {
                _ = output.AppendLine($"[{entry.Offset,8}] ({entry.Checksum,10}) {entry.Value}");
            }

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.strings.txt";
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }

        public void Read(byte[] data, Codec codec)
        {
            using BinaryReader input = new BinaryReader(new MemoryStream(data));

            uint stringTableLength = input.ReadUInt32();

            List<StringTableEntry> entries = Utility.CreateList<StringTableEntry>((int)input.ReadUInt32()).ToList();
            long seekOrigin = input.BaseStream.Position;

            foreach (StringTableEntry entry in entries)
            {
                entry.Checksum = input.ReadUInt32();
                entry.Offset = input.ReadUInt32();
            }

            foreach (StringTableEntry entry in entries)
            {
                _ = input.BaseStream.Seek(seekOrigin + entry.Offset, SeekOrigin.Begin);
                ushort textLength = input.ReadUInt16();
                byte[] textBytes = input.ReadBytes(textLength);

                entry.Value = Encoding.UTF8.GetString(textBytes);

                if (!entry.Validate())
                {
                    throw new FormatException($"The CRC32 for text table entry does not match. {entry}");
                }
            }

            Entries = new List<StringTableEntry>(entries);
        }

        public void Validate(Codec codec)
        {
            foreach (StringTableEntry entry in Entries)
            {
                if (!entry.Validate()) { throw new Exception($"Invalid Checksum for: {entry}"); }
            }
        }

        public void Write(BinaryWriter output, Codec codec)
        {
            List<StringTableEntry> stringTable = new List<StringTableEntry>();
            StringBuilder dataBuffer = new StringBuilder();

            //TODO: verify that this isn't needed because of a read error
            stringTable.Add(new StringTableEntry("", 0, 0));

            foreach (IRecord record in codec.Container) { stringTable.Add(new StringTableEntry(record.Name)); }


            stringTable = stringTable
                          .Distinct()
                          .OrderBy(s => s.Checksum)
                          .ToList();

            codec.Header.MaxKeyLength = stringTable.Max(s => s.Value.Length);

            MemoryStream bufferStream = new MemoryStream();
            using BinaryWriter buffer = new BinaryWriter(bufferStream);

            // First - Write out the [Content] section - needed so we can know the offsets.
            bufferStream.Position = HEADER_LENGTH + INDEX_ENTRY_LENGTH * stringTable.Count;

            foreach (StringTableEntry entry in stringTable)
            {
                entry.Offset = (uint)bufferStream.Position - HEADER_LENGTH;
                buffer.Write((ushort)entry.Value.Length);
                buffer.Write(Encoding.UTF8.GetBytes(entry.Value));
            }

            // Second - Write out the [Index] Section
            bufferStream.Position = 4 + ENTRY_COUNT_LENGTH;

            foreach (StringTableEntry entry in stringTable)
            {
                buffer.Write(entry.Checksum);
                buffer.Write(entry.Offset);
            }

            //Finally - Write out the [Header] section
            bufferStream.Position = 0;
            buffer.Write((uint)bufferStream.Length);
            buffer.Write((ushort)stringTable.Count);


            byte[] data = bufferStream.ToArray();
            codec.Header.StringTableLength = (uint)data.Length;

            output.Write(data);
        }

        public ushort IndexOf(string entryValue)
        {
            for (int index = 0; index < Entries.Count; index++)
            {
                if (Entries[index].Value == entryValue) { return (ushort)index; }
            }

            throw new KeyNotFoundException($"The entryValue {entryValue} could not be found in the StringTable");
        }
    }
}