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
using System.Text;
using Aaron.Core.CommandLine;
using Aaron.Core.Extensions;
using Aaron.Core.JsonConfig;

namespace Aaron.MassEffect.CommandLine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ShowBanner();

            MassEffectCliConfiguration defaultConfig = new MassEffectCliConfiguration();
            defaultConfig.GameLocation =
                @"C:\Program Files (x86)\Steam\steamapps\common\Mass Effect Legendary Edition\Game\";

            ConfigurationHost.Instance.Load<MassEffectCliConfiguration>("me/config");

            ArgumentParser parser = new ArgumentParser();

            new CommandFactory(parser.Commands)
                .AddConfigCommand()
                .AddOptionCommand()
                .AddSaveCommand();

            //TODO: Debug

#if DEBUG
            string[] debugArgs = { "option" };

            parser.Run(debugArgs);
#else
            parser.Run(args);
#endif
        }

        private static void ShowBanner()
        {
            List<string> emoji = new List<string>(Emoji.ListEmoji());

            foreach (string emote in emoji)
            {
                Console.WriteLine(emote);
            }

            return;


            string smallFlag = "🇺🇸";
            byte[] data = Encoding.UTF8.GetBytes("US");
            byte[] flagData = Encoding.UTF8.GetBytes(smallFlag);
            Console.WriteLine(flagData.ToByteString());
            Console.WriteLine(data.ToByteString());
            Console.WriteLine();

            string smallCloud = "☁";
            string bigCloud = "☁️";

            byte[] smallData = Encoding.UTF8.GetBytes(smallCloud);
            byte[] bigData = Encoding.UTF8.GetBytes(bigCloud);

            Console.WriteLine(smallData.ToByteString());
            Console.WriteLine(bigData.ToByteString());

            char star = '⭐';
            char cloud = '☁';
            string cloudString1 = cloud.ToString();
            string cloudString2 = "☁️";
            int cloudLength1 = cloudString1.Length;
            int cloudLength2 = cloudString2.Length;

            Console.OutputEncoding = Encoding.UTF8;

            ShowRuler(100);
            Console.WriteLine("⭐☁️");
            Console.WriteLine(star);
            Console.WriteLine(cloud);


            string ok = "hello".Center("☁️", 10);

            Console.WriteLine(ok);

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║       ⭐ Mass Effect Editor ⭐       ║");
            Console.WriteLine("║ Copyright ©️ 2021 - Aaron C. Willows ║");
            Console.WriteLine("╟──────────────────────────────────────╢");
            Console.WriteLine();
        }


        private static void ShowRuler(int width)
        {
            int ten = width / 10;

            StringBuilder line1 = new StringBuilder();
            StringBuilder line2 = new StringBuilder();
            for (int i = 0; i < ten; i++)
            {
                line1.Append(i.ToString().PadRight(10, '-'));
                line2.Append("0123456789");
            }

            Console.WriteLine(line1);
            Console.WriteLine(line2);
        }

        private class MassEffectCliConfiguration : IConfigurable
        {
            public string GameLocation { get; set; }

            public IConfigurable Copy()
            {
                MassEffectCliConfiguration copy = new MassEffectCliConfiguration
                {
                    GameLocation = GameLocation,
                };

                return copy;
            }
        }
    }
}