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

/*
 *
 * Copyright (c) 2012 Rick (rick 'at' gibbed 'dot' us)
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

using System.Collections;
using System.Text;

namespace Aaron.Binary.Compression.Huffman
{
    public static class Decoder
    {
        public static string Decode(Pair[] tree, BitArray data, int offset, int maxLength)
        {
            StringBuilder sb = new StringBuilder();

            int start = tree.Length - 1;
            while (true)
            {
                int node = start;
                do
                {
                    node = data[offset] == false
                        ? tree[node].Left
                        : tree[node].Right;
                    offset++;
                } while (node >= 0);

                ushort c = (ushort)(-1 - node);
                if (c == 0) { break; }

                sb.Append((char)c);
                if (sb.Length >= maxLength) { break; }
            }

            return sb.ToString();
        }
    }
}