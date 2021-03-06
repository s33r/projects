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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aaron.Core.Extensions
{
    public static class StringExtension
    {
        public static string Center(this string str, int totalLength, string padding)
        {
            if (str is null) { throw new ArgumentNullException(nameof(str)); }

            return Center(str, totalLength, str.Length, padding);
        }

        public static string Center(this string str, int totalLength, int overrideStringLength, string padding)
        {
            if (str is null) { throw new ArgumentNullException(nameof(str)); }

            if (padding is null) { throw new ArgumentNullException(nameof(padding)); }


            int paddingCount = totalLength - overrideStringLength;
            if (paddingCount % 2 != 0) { paddingCount++; }

            paddingCount = paddingCount / padding.Length / 2;

            int leftPad = paddingCount;
            int rightPad = totalLength - (paddingCount + overrideStringLength);

            if (paddingCount <= 0) { return str; }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < leftPad; i++) { stringBuilder.Append(padding); }

            stringBuilder.Append(str);

            for (int i = 0; i < rightPad; i++) { stringBuilder.Append(padding); }

            return stringBuilder.ToString();
        }

        public static string Mask(this string str, char mask, IEnumerable<char> whitelist)
        {
            if (str is null) { throw new ArgumentNullException(nameof(str)); }

            HashSet<char> hash = new HashSet<char>(whitelist.Distinct());
            char[] chars = str.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (!hash.Contains(chars[i])) { chars[i] = mask; }
            }

            return new string(chars);
        }

        public static string Mask(this string str, char mask)
        {
            return Mask(str, mask, Array.Empty<char>());
        }

        public static string Mask(this string str, char mask, char keep)
        {
            return Mask(str, mask, new[] { keep });
        }

        public static string PadLeft(this string str, int totalLength, int overrideStringLength, string padding)
        {
            if (str is null) { throw new ArgumentNullException(nameof(str)); }

            if (padding is null) { throw new ArgumentNullException(nameof(padding)); }

            int paddingCount = (totalLength - overrideStringLength) / padding.Length;

            if (paddingCount <= 0) { return str; }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < paddingCount; i++) { stringBuilder.Append(padding); }

            stringBuilder.Append(str);

            return stringBuilder.ToString();
        }
    }
}