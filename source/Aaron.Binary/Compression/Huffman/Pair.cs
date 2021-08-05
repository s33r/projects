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

// The code below has been updated to fit within this project and (possibly) to fix bugs.

/* Copyright (c) 2012 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;

namespace Aaron.Binary.Compression.Huffman
{
    public class Pair : IComparable, IComparable<Pair>, IEquatable<Pair>
    {
        public int Left { get; set; }
        public int Right { get; set; }

        public Pair()
            : this(0, 0) { }

        public Pair(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((Pair)obj);
        }

        public int CompareTo(Pair other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            if (other.Left > Left) { return -1; }

            if (other.Left < Left) { return 1; }

            if (other.Right == Right) { return 0; }

            if (other.Right > Right) { return -1; }

            return 1;
        }

        public bool Equals(Pair other)
        {
            if (other is null) { return false; }

            if (ReferenceEquals(this, other)) { return true; }

            return Left == other.Left && Right == other.Right;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) { return false; }

            if (ReferenceEquals(this, obj)) { return true; }

            if (obj.GetType() != GetType()) { return false; }

            return Equals((Pair)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Right);
        }

        public static bool operator ==(Pair left, Pair right)
        {
            if (left is null) { return right is null; }

            return left.Equals(right);
        }

        public static bool operator >(Pair left, Pair right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Pair left, Pair right)
        {
            return left is null
                ? right is null
                : left.CompareTo(right) >= 0;
        }

        public static bool operator !=(Pair left, Pair right)
        {
            return !(left == right);
        }

        public static bool operator <(Pair left, Pair right)
        {
            return left is null
                ? right is not null
                : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Pair left, Pair right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }
    }
}