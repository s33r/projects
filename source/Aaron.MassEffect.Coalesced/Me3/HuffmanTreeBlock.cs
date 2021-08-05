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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aaron.Binary.Compression.Huffman;
using Aaron.MassEffect.Core;
using Encoder = Aaron.Binary.Compression.Huffman.Encoder;

namespace Aaron.MassEffect.Coalesced.Me3
{
    internal class HuffmanTreeBlock : IBlock<Codec>
    {
        public const int HUFFMAN_TUPLE_SIZE = 8;

        public Encoder Encoder { get; private set; }
        public List<Pair> HuffmanTuples { get; private set; }

        public string Dump()
        {
            StringBuilder output = new StringBuilder()
                .AppendLine($"Count = {HuffmanTuples.Count}");

            foreach (Pair tuple in HuffmanTuples) { _ = output.AppendLine($"({tuple.Left,8}, {tuple.Right,8})"); }

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.huffman.txt";
            string outputLocation = Path.Join(MassEffectConfiguration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }

        public void Read(byte[] data, Codec codec)
        {
            using BinaryReader input = new BinaryReader(new MemoryStream(data));

            ushort entryCount = input.ReadUInt16();

            List<Pair> pairs = new List<Pair>();

            for (int currentEntry = 0; currentEntry < entryCount; currentEntry++)
            {
                Pair pair = new Pair
                {
                    Left = input.ReadInt32(),
                    Right = input.ReadInt32(),
                };

                pairs.Add(pair);
            }

            HuffmanTuples = pairs;
        }

        public void Validate(Codec codec) { }

        public void Write(BinaryWriter output, Codec codec)
        {
            string data = codec.Container.GetData(out int maxValueLength);
            codec.Header.MaxValueLength = maxValueLength;

            Encoder = new Encoder();
            Encoder.Build(data);

            HuffmanTuples = Encoder.GetPairs().ToList();
            codec.Header.HuffmanLength = (uint)(HUFFMAN_TUPLE_SIZE * HuffmanTuples.Count) + 2;

            output.Write((ushort)HuffmanTuples.Count);

            foreach (Pair pair in HuffmanTuples)
            {
                output.Write(pair.Left);
                output.Write(pair.Right);
            }
        }
    }
}