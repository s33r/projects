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
using Aaron.Core.CommandLine;
using Aaron.Core.JsonConfig;

namespace Aaron.MassEffect.CommandLine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ShowBanner();

            MassEffectCliConfiguration defaultConfig = new MassEffectCliConfiguration
            {
                GameLocation =
                    @"C:\Program Files (x86)\Steam\steamapps\common\Mass Effect Legendary Edition\Game\",
            };

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
            foreach (string emote in Emoji.ListEmoji()) { Console.WriteLine($"[{emote.Length}] {emote}  |"); }

            Render.Write(Render.ColumnHeaders());
            Render.Write(Render.Banner(
                $"{Emoji.Star} Mass Effect Editor {Emoji.Star}", //20 + 2 + 2
                "Copyright ©️ 2021 - Aaron C. Willows",
                "This is a message"
            ));
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