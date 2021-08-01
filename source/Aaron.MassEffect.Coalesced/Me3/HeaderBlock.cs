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
using System.IO;
using System.Text;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced.Me3
{
    internal class HeaderBlock : IBlock<Codec>
    {
        public const int HEADER_LENGTH = 8 * 4;
        public const uint MAGIC_WORD = 0x666D726D;

        public uint DataLength { get; set; }

        public uint HuffmanLength { get; set; }

        public uint IndexLength { get; set; }

        public uint MagicWord { get; set; }

        public int MaxKeyLength { get; set; }
        public int MaxValueLength { get; set; }

        public uint StringTableLength { get; set; }
        public uint Version { get; set; }

        public string Dump()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine($"          MagicWord = {MagicWord,16}");
            output.AppendLine($"            Version = {Version,16}");
            output.AppendLine($"       MaxKeyLength = {MaxKeyLength,16}");
            output.AppendLine($"     MaxValueLength = {MaxValueLength,16}");
            output.AppendLine($"  StringTableLength = {StringTableLength,16}");
            output.AppendLine($"      HuffmanLength = {HuffmanLength,16}");
            output.AppendLine($"        IndexLength = {IndexLength,16}");
            output.AppendLine($"         DataLength = {DataLength,16}");

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.header.txt";
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }

        public void Read(byte[] data, Codec codec)
        {
            BinaryReader input = new BinaryReader(new MemoryStream(data));

            Read(input, codec);
        }


        public void Validate(Codec codec)
        {
            if (MagicWord != MAGIC_WORD)
            {
                throw new FormatException($"The file does not begin with the correct magic word 0x{MAGIC_WORD:X}");
            }

            if (Version != 1) { throw new FormatException("We can only parse version 1 of this file"); }
        }


        public void Write(BinaryWriter output, Codec codec)
        {
            MagicWord = MAGIC_WORD;
            Version = 1;

            output.Write(MagicWord);
            output.Write(Version);
            output.Write(MaxKeyLength);
            output.Write(MaxValueLength);
            output.Write(StringTableLength);
            output.Write(HuffmanLength);
            output.Write(IndexLength);
            output.Write(DataLength);
        }

        public bool Match(HeaderBlock other)
        {
            return MagicWord == other.MagicWord
                   && Version == other.Version
                   && MaxKeyLength == other.MaxKeyLength
                   && MaxValueLength == other.MaxValueLength
                   && StringTableLength == other.StringTableLength
                   && HuffmanLength == other.HuffmanLength
                   && IndexLength == other.IndexLength
                   && DataLength == other.DataLength;
        }


        public void Read(BinaryReader input, Codec codec)
        {
            MagicWord = input.ReadUInt32();
            Version = input.ReadUInt32();
            Validate(codec);
            MaxKeyLength = input.ReadInt32();
            MaxValueLength = input.ReadInt32();
            StringTableLength = input.ReadUInt32();
            HuffmanLength = input.ReadUInt32();
            IndexLength = input.ReadUInt32();
            DataLength = input.ReadUInt32();
        }
    }
}