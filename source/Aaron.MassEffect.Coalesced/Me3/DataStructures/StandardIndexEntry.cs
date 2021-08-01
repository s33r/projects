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

namespace Aaron.MassEffect.Coalesced.Me3.DataStructures
{
    internal class StandardIndexEntry
    {
        public uint Offset { get; set; }
        public ushort StringTableIndex { get; set; }

        public StandardIndexEntry() { }

        public StandardIndexEntry(ushort stringTableIndex)
        {
            StringTableIndex = stringTableIndex;
        }

        public StandardIndexEntry(ushort stringTableIndex, uint offset)
        {
            StringTableIndex = stringTableIndex;
            Offset = offset;
        }

        public string GetString(StringTableBlock stringTable)
        {
            return stringTable.Entries[StringTableIndex].Value;
        }

        public void Read(BinaryReader reader)
        {
            StringTableIndex = reader.ReadUInt16();
            Offset = reader.ReadUInt32();
        }

        public static int Size()
        {
            return 6;
        }

        public override string ToString()
        {
            return $"{Offset} | {StringTableIndex}";
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(StringTableIndex);
            writer.Write(Offset);
        }
    }
}