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
using System.Globalization;
using System.Text;

namespace Aaron.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static string ToByteString(this byte[] bytes, int start, int length)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = start; i < length + start; i++)
            {
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0:X2} ", bytes[i]);
            }

            return stringBuilder.ToString().Trim();
        }

        public static string ToByteString(this byte[] bytes, int start)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }

            return ToByteString(bytes, start, bytes.Length);
        }

        public static string ToByteString(this byte[] bytes)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }

            return ToByteString(bytes, 0, bytes.Length);
        }
    }
}