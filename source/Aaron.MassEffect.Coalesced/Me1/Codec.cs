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

using System.IO;
using System.Linq;
using System.Text;
using Aaron.MassEffect.Coalesced.Records;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced.Me1
{
    internal class Codec : ICodec
    {
        public const int DEFAULT_STRING = 0;

        public Codec(string name)
        {
            Name = name;
        }

        public Codec()
            : this("Mass Effect 1 Codec") { }

        public Container Decode(byte[] value)
        {
            Container container = new Container();
            using BinaryReader input = new BinaryReader(new MemoryStream(value));

            int fileCount = input.ReadInt32();
            for (int fileIndex = 0; fileIndex < fileCount; fileIndex++)
            {
                FileRecord fileRecord = new FileRecord { Name = ReadString(input) };

                int sectionCount = input.ReadInt32();
                for (int sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++)
                {
                    SectionRecord sectionRecord = new SectionRecord { Name = ReadString(input) };

                    int entryCount = input.ReadInt32();
                    for (int entryIndex = 0; entryIndex < entryCount; entryIndex++)
                    {
                        EntryRecord entryRecord = new EntryRecord { Name = ReadString(input) };
                        entryRecord.Add(ReadString(input));

                        sectionRecord.Add(entryRecord);
                    }

                    fileRecord.Add(sectionRecord);
                }

                container.Files.Add(fileRecord);
            }

            return container;
        }

        public void Dump() { }

        public byte[] Encode(Container value)
        {
            MemoryStream memoryStream = new MemoryStream();
            using BinaryWriter output = new BinaryWriter(memoryStream);

            output.Write(value.Files.Count);
            foreach (FileRecord fileRecord in value.Files)
            {
                WriteString(fileRecord.Name, output);

                output.Write(fileRecord.Count);
                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    WriteString(sectionRecord.Name, output);

                    output.Write(fileRecord.Count);
                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        WriteString(entryRecord.Name, output);
                        WriteString(entryRecord.First(), output);
                    }
                }
            }

            return memoryStream.ToArray();
        }

        public Games Game => Games.Me1;

        public string Name { get; }

        private static string ReadString(BinaryReader input)
        {
            int stringLength = input.ReadInt32() * -2; //string lengths are the negative number of characters?

            if (stringLength == 0) { return string.Empty; }

            byte[] stringBuffer = input.ReadBytes(stringLength);

            return Encoding.Unicode.GetString(stringBuffer, 0, stringBuffer.Length - 2);
        }

        private static void WriteString(string value, BinaryWriter output)
        {
            if (string.IsNullOrEmpty(value))
            {
                output.Write(DEFAULT_STRING);
                return;
            }

            int stringLength = (value.Length + 1) * -1;
            byte[] stringBuffer = Encoding.Unicode.GetBytes(value + '\0');

            output.Write(stringLength);
            output.Write(stringBuffer);
        }
    }
}