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
using Aaron.Binary.Compression.Huffman;

namespace Aaron.MassEffect.Coalesced.Me3.DataStructures
{
    internal class Item
    {
        public ushort Count;
        public StandardIndexEntry Parent;
        public int[] Values;

        public Item() { }

        public Item(ushort count, StandardIndexEntry parent)
        {
            Parent = parent;
            Count = count;
            Values = new int[count];
        }

        public string Decode(int index, HuffmanTreeBlock huffmanTree, BitArray compressedData, int maxValueLength)
        {
            int offset = Values[index];
            long type = (offset & 0xE0000000) >> 29;

            if (type == 1) { return null; }

            if (type == 2)
            {
                offset &= 0x1FFFFFFF;
                string text = Decoder.Decode(huffmanTree.HuffmanTuples.ToArray(), compressedData, offset,
                    maxValueLength);

                return text;
            }

            throw new InvalidDataException("Unknown compression type");
        }

        public List<string> Decode(HuffmanTreeBlock huffmanTree, BitArray compressedData, int maxValueLength)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < Values.Length; i++)
            {
                result.Add(Decode(i, huffmanTree, compressedData, maxValueLength));
            }

            return result;
        }

        public int Encode(string item, int index, int bitOffset, Encoder encoder, BitArray compressedData)
        {
            int value;
            int newBitOffset = bitOffset;


            if (item == null) { value = (1 << 29) | bitOffset; }
            else
            {
                value = (2 << 29) | bitOffset;
                newBitOffset += encoder.Encode(item + '\0', compressedData, bitOffset);
            }

            Values[index] = value;

            return newBitOffset;
        }

        public void Read(BinaryReader reader, StandardIndexEntry parent)
        {
            Parent = parent;
            Count = reader.ReadUInt16();
            Values = new int[Count];

            for (int i = 0; i < Count; i++) { Values[i] = reader.ReadInt32(); }
        }

        public ushort Size()
        {
            return Size(Count);
        }

        public static ushort Size(int count)
        {
            return (ushort)(2 + 4 * count);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Count);

            foreach (int value in Values) { writer.Write(value); }
        }
    }
}