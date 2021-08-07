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
using Aaron.MassEffect.CommandLine.CommandOption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.MassEffect.CommandLine.Tests
{
    [TestClass]
    public class TestSimpleMatching
    {
        private static readonly List<string> TEST_DATA = new List<string>
        {
            @"..\..\BIOGame\Config\BIOCompat.ini",
            @"..\..\BIOGame\Config\BIOCredits.ini",
            @"..\..\BIOGame\Config\BIOEditor.ini",
            @"..\..\BIOGame\Config\BIOEditorKeyBindings.ini",
            @"..\..\BIOGame\Config\BIOEditorUserSettings.ini",
            @"..\..\BIOGame\Config\BIOEngine.ini",
            @"..\..\BIOGame\Config\BIOGame.ini",
            @"..\..\BIOGame\Config\BIOInput.ini",
            @"..\..\BIOGame\Config\BIOLightmass.ini",
            @"..\..\BIOGame\Config\BIOParty.ini",
            @"..\..\BIOGame\Config\BIOQA.ini",
            @"..\..\BIOGame\Config\BIOStringTypeMap.ini",
            @"..\..\BIOGame\Config\BIOUI.ini",
            @"..\..\BIOGame\Config\BIOWeapon.ini",
            @"..\..\Engine\Localization\INT\Core.int",
            @"..\..\Engine\Localization\INT\Descriptions.int",
            @"..\..\Engine\Localization\INT\EditorTips.int",
            @"..\..\Engine\Localization\INT\Engine.int",
            @"..\..\Engine\Localization\INT\GFxUI.int",
            @"..\..\Engine\Localization\INT\IpDrv.int",
            @"..\..\Engine\Localization\INT\Launch.int",
            @"..\..\Engine\Localization\INT\OnlineSubsystemGameSpy.int",
            @"..\..\Engine\Localization\INT\Startup.int",
            @"..\..\Engine\Localization\INT\Subtitles.int",
            @"..\..\Engine\Localization\INT\UnrealEd.int",
            @"..\..\Engine\Localization\INT\WinDrv.int",
            @"..\..\Engine\Localization\INT\XWindow.int",
        };


        [TestMethod]
        public void DontMatchNoValueNoPattern()
        {
            string pattern = null;
            string value = null;

            bool result = Finder.SimpleMatch(pattern, value);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void DontMatchNoValuePattern()
        {
            string pattern = "something";
            string value = null;

            bool result = Finder.SimpleMatch(pattern, value);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void MatchAll()
        {
            string pattern = @"*";

            List<Tuple<bool, string>> results = Find(pattern);

            Assert.AreEqual(TEST_DATA.Count, results.Select(t => t.Item1).Count(i => i));
        }

        [TestMethod]
        public void MatchAllEmptyValue()
        {
            string pattern = null;
            string value = "something";

            bool result = Finder.SimpleMatch(pattern, value);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void MatchCrazy()
        {
            string pattern = @"*Game*BIO*.ini";

            bool testResult = Finder.SimpleMatch(pattern, @"..\..\BIOGame\Config\BIOEngine.ini");

            Assert.AreEqual(true, testResult);
        }

        [TestMethod]
        public void MatchEnd()
        {
            string pattern = @"*.ini";

            bool testResult = Finder.SimpleMatch(pattern, @"..\..\BIOGame\Config\BIOEngine.ini");

            Assert.AreEqual(true, testResult);
        }


        [TestMethod]
        public void MatchInfix()
        {
            string pattern = @"*Engine*";

            bool testResult = Finder.SimpleMatch(pattern, @"..\..\Enginex");

            Assert.AreEqual(true, testResult);
        }

        [TestMethod]
        public void MatchPostfix()
        {
            string pattern = @"..\..\BIOGame\config*";

            bool testResult = Finder.SimpleMatch(pattern, @"..\..\BIOGame\Config\BIOEngine.ini");

            Assert.AreEqual(true, testResult);
        }


        [TestMethod]
        public void MatchPrefix()
        {
            string pattern = @"*Engine";

            bool testResult = Finder.SimpleMatch(pattern, @"..\..\Engine");

            Assert.AreEqual(true, testResult);
        }

        [TestMethod]
        public void NoMatch()
        {
            string pattern = @"Config\BIOCompat.ini";

            bool testResult = Finder.SimpleMatch(pattern, @"Config\BIOEngine.ini");

            Assert.AreEqual(false, testResult);
        }


        private static List<Tuple<bool, string>> Find(string pattern)
        {
            return TEST_DATA
                   .Select(d => new Tuple<bool, string>(Finder.SimpleMatch(pattern, d), d))
                   .ToList();
        }
    }
}