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

// is this code terrible?
// yup!


/*
 * Source modified to align with the rest of the projects conventions. - Aaron
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aaron.Binary.Compression.Huffman
{
    public class Encoder
    {
        private readonly Dictionary<char, BitArray> _codes = new Dictionary<char, BitArray>();
        private Node _root;

        public int TotalBits { get; private set; }

        public void Build(string text)
        {
            if (text == null) { throw new ArgumentNullException(nameof(text)); }

            _root = null;
            Dictionary<char, int> frequencies = new Dictionary<char, int>();
            _codes.Clear();

            foreach (char t in text)
            {
                if (frequencies.TryGetValue(t, out int frequency) == false) { frequency = 0; }

                frequencies[t] = frequency + 1;
            }

            List<Node> nodes = frequencies.Select(
                symbol => new Node { Symbol = symbol.Key, Frequency = symbol.Value }).ToList();

            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes
                                          .OrderBy(n => n.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    Node[] taken = orderedNodes.Take(2).ToArray();
                    Node first = taken[0];
                    Node second = taken[1];

                    Node parent = new Node
                    {
                        Symbol = '\0', Frequency = first.Frequency + second.Frequency, Left = first, Right = second,
                    };

                    _ = nodes.Remove(first);
                    _ = nodes.Remove(second);
                    nodes.Add(parent);
                }

                _root = nodes.FirstOrDefault();
            }

            foreach (KeyValuePair<char, int> frequency in frequencies)
            {
                List<bool> bits = Traverse(_root, frequency.Key, new List<bool>());
                if (bits == null) { throw new InvalidOperationException($"could not traverse '{frequency.Key}'"); }

                _codes.Add(frequency.Key, new BitArray(bits.ToArray()));
            }

            TotalBits = GetTotalBits(_root);
        }

        public int Encode(string text, BitArray bits, int offset)
        {
            if (text == null) { throw new ArgumentNullException(nameof(text)); }

            if (bits == null) { throw new ArgumentNullException(nameof(bits)); }


            int bitCount = 0;
            foreach (char t in text)
            {
                if (_codes.ContainsKey(t) == false)
                {
                    throw new ArgumentException($"could not lookup '{t}'", nameof(text));
                }

                bitCount += Encode(t, bits, offset + bitCount);
            }

            return bitCount;
        }

        public Pair[] GetPairs()
        {
            List<Pair> pairs = new List<Pair>();
            Dictionary<Node, Pair> mapping = new Dictionary<Node, Pair>();

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(_root);

            Pair root = new Pair();
            mapping.Add(_root, root);

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                Pair pair = mapping[node];

                if (node.Left == null && node.Right == null) { throw new InvalidOperationException(); }

                // ReSharper disable PossibleNullReferenceException
                if (node.Left.Left == null &&
                    // ReSharper restore PossibleNullReferenceException
                    node.Left.Right == null) { pair.Left = -1 - node.Left.Symbol; }
                else
                {
                    Pair left = new Pair();
                    mapping.Add(node.Left, left);
                    pairs.Add(left);

                    queue.Enqueue(node.Left);

                    pair.Left = pairs.IndexOf(left);
                }

                if (node.Right.Left == null &&
                    node.Right.Right == null) { pair.Right = -1 - node.Right.Symbol; }
                else
                {
                    Pair right = new Pair();
                    mapping.Add(node.Right, right);
                    pairs.Add(right);

                    queue.Enqueue(node.Right);

                    pair.Right = pairs.IndexOf(right);
                }
            }

            pairs.Add(root);
            return pairs.ToArray();
        }

        private int Encode(char symbol, BitArray bits, int offset)
        {
            BitArray code = _codes[symbol];
            for (int i = 0; i < code.Length; i++) { bits[offset + i] = code[i]; }

            return code.Length;
        }

        private static int GetTotalBits(Node root)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);

            int totalBits = 0;
            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                if (node.Left == null && node.Right == null) { continue; }

                totalBits += node.Frequency;

                if (node.Left != null &&
                    node.Left.Left != null &&
                    node.Left.Right != null) { queue.Enqueue(node.Left); }

                if (node.Right != null &&
                    node.Right.Left != null &&
                    node.Right.Right != null) { queue.Enqueue(node.Right); }
            }

            return totalBits;
        }

        private static List<bool> Traverse(Node node, char symbol, List<bool> data)
        {
            if (node.Left == null &&
                node.Right == null)
            {
                return symbol == node.Symbol
                    ? data
                    : null;
            }

            if (node.Left != null)
            {
                List<bool> path = new List<bool>();
                path.AddRange(data);
                path.Add(false);

                List<bool> left = Traverse(node.Left, symbol, path);
                if (left != null) { return left; }
            }

            if (node.Right != null)
            {
                List<bool> path = new List<bool>();
                path.AddRange(data);
                path.Add(true);

                List<bool> right = Traverse(node.Right, symbol, path);
                if (right != null) { return right; }
            }

            return null;
        }
    }
}