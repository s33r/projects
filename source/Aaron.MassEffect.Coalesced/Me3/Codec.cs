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
using System.IO;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced.Me3
{
    internal class Codec : ICodec
    {
        public BitArray CompressedData { get; set; }

        public Container Container { get; set; }
        public DataBlock Data { get; } = new DataBlock();

        public Games Game => Games.Me3;

        public HeaderBlock Header { get; } = new HeaderBlock();
        public HuffmanTreeBlock HuffmanTree { get; } = new HuffmanTreeBlock();
        public StringTableBlock StringTable { get; } = new StringTableBlock();

        public Codec(string name)
        {
            Name = name;
        }

        public Codec()
            : this("Mass Effect 3 Codec") { }

        public Container Decode(byte[] value)
        {
            BinaryReader input = new BinaryReader(new MemoryStream(value));

            Header.Read(input, this);
            StringTable.Read(input.ReadBytes((int)Header.StringTableLength), this);
            HuffmanTree.Read(input.ReadBytes((int)Header.HuffmanLength), this);

            byte[] indexData = input.ReadBytes((int)Header.IndexLength);

            int compressedDataLength =
                input.ReadInt32(); //TODO: This should really be in the DataBlock, its just easier to do it here because if the interface
            CompressedData = new BitArray(input.ReadBytes((int)Header.DataLength));

            Data.Read(indexData, this);

            Container = Data.Container;

            return Data.Container;
        }

        public void Dump()
        {
            Header.Dump(Name);
            StringTable.Dump(Name);
            HuffmanTree.Dump(Name);
            Data.Dump(Name);
        }

        public byte[] Encode(Container value)
        {
            Container = value;

            MemoryStream outputStream = new MemoryStream();
            BinaryWriter output = new BinaryWriter(outputStream);
            outputStream.Position = HeaderBlock.HEADER_LENGTH;

            StringTable.Write(output, this);
            HuffmanTree.Write(output, this);
            Data.Write(output, this);

            outputStream.Position = 0;
            Header.Write(output, this);

            outputStream.Flush();
            return outputStream.ToArray();
        }

        public string Name { get; set; }
    }
}