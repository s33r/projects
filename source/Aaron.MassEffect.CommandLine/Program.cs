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

using System.Linq;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;
using Aaron.Core.JsonConfig;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.CommandLine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Render.InitializeConsole();
            ShowBanner();

            ConfigurationHost.Instance.Load<MassEffectCliConfiguration>("config");
            MassEffectConfiguration.Instance.Initialize();

            ArgumentParser parser = new ArgumentParser();

            new CommandFactory(parser.Commands)
                .AddConfigCommand()
                .AddOptionCommand()
                .AddSaveCommand();

#if DEBUG
            string[] debugArgs = { "option", "get", "-t", "simple", "-f", @"*.ini" };
            ParsedCommandLine commandLine = parser.Parse(debugArgs);
#else
            ParsedCommandLine commandLine = parser.Parse(args);
#endif

            if (commandLine.HasErrors) { Render.Write(commandLine.Errors.Select(error => error.ToString())); }

            if (commandLine.HasFatalErrors) { return; }


            parser.Run(commandLine);
        }

        private static void ShowBanner()
        {
#if DEBUG
            Render.Write(Render.ColumnHeaders());
#endif
            Render.Write(Render.Banner(
                $"{Emoji.StarGlow} Mass Effect Editor {Emoji.StarGlow}", //20 + 2 + 2
                "Copyright ©️ 2021 - Aaron C. Willows",
                "Alpha 1"
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