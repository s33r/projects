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

using System.Collections.Generic;
using Aaron.Core.CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Core.Tests.CommandLine
{
    [TestClass]
    public class TestEmoji
    {
        [TestMethod]
        public void DuplicateEmoji()
        {
            HashSet<string> emojiSet = new HashSet<string>();

            foreach (string emote in Emoji.ListEmoji())
            {
                if (emojiSet.Contains(emote)) { Assert.Fail($"Duplicate emoji found ( {emote} )!"); }
                else { emojiSet.Add(emote); }
            }
        }
    }
}