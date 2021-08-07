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

// NOTE: Its much better to edit this file in VSCode since Visual Studio doesn't handle emojis as 
//       clearly.

// NOTE: Be sure to get the full width emoji here so that the console doesn't make it tiny.

// Collected from https://unicode.org/Public/emoji/13.1/emoji-sequences.txt


namespace Aaron.Core.CommandLine
{
    public static partial class Emoji
    {
#pragma warning disable format

        public static string Cloud     { get; } = "☁️";
        public static string MoonFull  { get; } = "🌝";
        public static string MoonLeft  { get; } = "🌜";
        public static string MoonRight { get; } = "🌛";
        public static string Star      { get; } = "⭐";
        public static string StarGlow  { get; } = "🌟";
        public static string Trophy    { get; } = "🏆";
        public static string Cross     { get; } = "❌";
        public static string Sparkles  { get; } = "✨";
        public static string Stopwatch { get; } = "⏱️";
        public static string Skull     { get; } = "☠️";
        public static string Gear      { get; } = "⚙️";
        public static string Folder    { get; } = "📁";
        public static string File      { get; } = "📃";
        public static string Search    { get; } = "🔍";

#pragma warning restore format
    }
}