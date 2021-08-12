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

namespace Aaron.Masseffect.UnrealPackage
{
    public class PackageHeader
    {
        public int DependsOffset { get; set; }

        public int ExportCount { get; set; }
        public int ExportOffset { get; set; }

        public string Group { get; set; }

        public int HeaderSize { get; set; }

        public int ImportCount { get; set; }
        public int ImportOffset { get; set; }
        public ushort Licensee { get; set; }

        public int NameCount { get; set; }
        public int NameHeader { get; set; }
        public int PackageFlags { get; set; }

        public Guid PackageGuid { get; set; }
        public int SerializedOffset { get; set; }
        public uint Signature { get; set; }

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public ushort Version { get; set; }


        public void Read(BinaryReader input)
        {
            Signature = input.ReadUInt32();
            Licensee = input.ReadUInt16();
            Version = input.ReadUInt16();
            HeaderSize = input.ReadInt32();
        }
    }
}