using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (emojiSet.Contains(emote))
                {
                    Assert.Fail($"Duplicate emoji found ( {emote} )!");
                }
                else
                {
                    emojiSet.Add(emote);
                }
            }

            
        }
    }
}
