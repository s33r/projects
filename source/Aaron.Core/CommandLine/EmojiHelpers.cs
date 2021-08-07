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

// This may be the worst thing I've ever done...

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Aaron.Core.CommandLine
{
    public static partial class Emoji
    {
        // ReSharper disable once InconsistentNaming
        private static readonly HashSet<string> _emojiSet = new HashSet<string>();

        // ReSharper disable once InconsistentNaming
        private static readonly Dictionary<string, int> _widthOverrides = new Dictionary<string, int>();

        static Emoji()
        {
            foreach (string emote in ListEmoji())
            {
                _emojiSet.Add(emote);
                _widthOverrides.Add(emote, emote.Length);
            }

            _widthOverrides[Star] = 2;
            _widthOverrides[Cross] = 2;
            _widthOverrides[Sparkles] = 2;
        }

        public static int CountEmoji(string text)
        {
            if (text is null) { throw new ArgumentNullException(nameof(text)); }

            int counter = 0;

            foreach (char c in text)
            {
                if (_emojiSet.Contains(c.ToString())) { counter++; }
            }

            return counter;
        }

        // This is needed because some terminals *cough* windows doesn't actually measure width correctly.
        // ... yeah its terrible and I hate it
        // https://github.com/microsoft/terminal/issues/8667
        public static int GetActualLength(string str)
        {
            if (str is null) { throw new ArgumentNullException(nameof(str)); }

            int length = 0;

            foreach (char c in str)
            {
                string charString = c.ToString();
                length += _widthOverrides.ContainsKey(charString)
                    ? _widthOverrides[charString]
                    : new StringInfo(charString).LengthInTextElements;
            }

            return length;
        }

        public static IEnumerable<string> ListEmoji()
        {
            IEnumerable<string> result = typeof(Emoji)
                                         .GetProperties(BindingFlags.Public | BindingFlags.Static)
                                         .Where(f => f.PropertyType == typeof(string))
                                         .Select(info => info.GetValue(null)?.ToString());

            return result;
        }
    }
}