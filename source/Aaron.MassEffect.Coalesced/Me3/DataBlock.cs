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
using System.IO;
using System.Text;
using Aaron.MassEffect.Coalesced.Me3.DataStructures;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced.Me3
{
    internal class DataBlock : IBlock<Codec>
    {
        public Container Container { get; set; } = new Container();

        public string Dump()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("Nothing here yet");

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.data.txt";
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }

        public void Read(byte[] data, Codec codec)
        {
            BinaryReader indexInput = new BinaryReader(new MemoryStream(data));

            IndexContainer indexContainer = new IndexContainer();
            indexContainer.Read(indexInput);
            indexContainer.Dump(codec.Name);

            Container = indexContainer.ToRecords(codec.StringTable, codec.HuffmanTree, codec.CompressedData,
                codec.Header.MaxValueLength);
        }

        public void Validate(Codec codec)
        {
            throw new NotImplementedException();
        }

        public void Write(BinaryWriter output, Codec codec)
        {
            MemoryStream bufferStream = new MemoryStream();
            BinaryWriter buffer = new BinaryWriter(bufferStream);

            BitArray compressedData = new(codec.HuffmanTree.Encoder.TotalBits);

            IndexContainer indexContainer = IndexContainer.FromRecords(Container, codec.StringTable,
                codec.HuffmanTree.Encoder, compressedData);
            int expectedLength = indexContainer.TotalSize();
            indexContainer.Write(buffer);

            byte[] indexData = bufferStream.ToArray();
            byte[] data = new byte[(compressedData.Length - 1) / 8 + 1];
            compressedData.CopyTo(data, 0);

            codec.Header.IndexLength = (uint)indexData.Length;
            codec.Header.DataLength = (uint)data.Length;

            //TODO - Seek?
            output.Write(indexData);
            output.Write(compressedData.Length);
            output.Write(data);
        }
    }
}