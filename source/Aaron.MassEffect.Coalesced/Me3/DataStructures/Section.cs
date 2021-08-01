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
    internal class Section
    {
        public Entry[] Entries;
        public StandardIndex Index { get; set; }

        public StandardIndexEntry Parent { get; set; }


        public Section() { }

        public Section(ushort count, StandardIndexEntry parent)
        {
            Parent = parent;

            Index = new StandardIndex(count);
            Entries = new Entry[count];

            for (int i = 0; i < count; i++) { Entries[i] = new Entry(); }
        }

        public void Read(BinaryReader reader, uint origin, StandardIndexEntry parent)
        {
            Parent = parent;

            Index = new StandardIndex();
            Index.Read(reader);

            Entries = new Entry[Index.Count];

            for (int i = 0; i < Index.Count; i++)
            {
                uint newOrigin = origin + Index[i].Offset;
                reader.BaseStream.Seek(newOrigin, SeekOrigin.Begin);

                Entries[i] = new Entry();
                Entries[i].Read(reader, newOrigin, Index[i]);
            }
        }

        public ushort Size()
        {
            return Size(Index.Count);
        }

        public static ushort Size(int count)
        {
            return StandardIndex.Size(count);
        }

        public void Write(BinaryWriter writer)
        {
            Index.Write(writer);

            foreach (Entry entryIndex in Entries) { entryIndex.Write(writer); }
        }
    }
}