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
    internal class StandardIndex
    {
        public ushort Count { get; set; }

        public StandardIndexEntry this[int index]
        {
            get => Table[index];
            set => Table[index] = value;
        }

        public StandardIndexEntry[] Table { get; set; }

        public StandardIndex() { }

        public StandardIndex(ushort count)
        {
            Table = new StandardIndexEntry[count];
            Count = count;

            for (int i = 0; i < count; i++) { Table[i] = new StandardIndexEntry(); }
        }


        public void Read(BinaryReader reader)
        {
            Count = reader.ReadUInt16();
            Table = new StandardIndexEntry[Count];

            for (int i = 0; i < Count; i++)
            {
                Table[i] = new StandardIndexEntry();
                Table[i].Read(reader);
            }
        }

        public ushort Size()
        {
            return Size(Count);
        }

        public static ushort Size(int count)
        {
            return (ushort)(2 + count * StandardIndexEntry.Size());
        }


        public override string ToString()
        {
            return $"{Count} ({Size()} bytes)";
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Count);

            foreach (StandardIndexEntry entry in Table) { entry.Write(writer); }
        }
    }
}