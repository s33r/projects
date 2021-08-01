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
using Aaron.Binary.Checksum;

namespace Aaron.MassEffect.Coalesced.Me3.DataStructures
{
    internal class StringTableEntry : IEquatable<StringTableEntry>
    {
        public uint Checksum { get; set; }
        public uint Offset { get; set; }
        public string Value { get; set; }

        public StringTableEntry() { }

        public StringTableEntry(string value, uint offset, uint checksum)
        {
            Value = value;
            Offset = offset;
            Checksum = checksum;
        }

        public StringTableEntry(string value, uint offset)
        {
            Value = value;
            Offset = offset;
            Checksum = Crc32.Compute(Value);
        }

        public StringTableEntry(string value)
        {
            Value = value;
            Checksum = Crc32.Compute(Value);
        }

        public bool Equals(StringTableEntry other)
        {
            if (other == null) { return false; }

            return other.Checksum == Checksum;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) { return false; }

            return Equals((StringTableEntry)obj);
        }

        public override int GetHashCode()
        {
            return (int)Crc32.Compute(Value);
        }

        public override string ToString()
        {
            return $"[Offset={Offset}][Checksum={Checksum:X}] {Value}";
        }

        public bool Validate()
        {
            uint crc32 = Crc32.Compute(Value);

            return crc32 == Checksum;
        }
    }
}