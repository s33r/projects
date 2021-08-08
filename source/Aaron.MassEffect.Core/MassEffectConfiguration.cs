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
using System.IO;

namespace Aaron.MassEffect.Core
{
    public sealed class MassEffectConfiguration
    {
        public Dictionary<Games, GameConfiguration> Game { get; } = new Dictionary<Games, GameConfiguration>();
        public string GameBaseLocation { get; set; }
        public string WorkingLocation { get; set; }

        public void Initialize()
        {
            if (!BitConverter.IsLittleEndian)
            {
                Console.WriteLine(
                    "The processor is not little endian... things are going to break!"); // TODO: Handle big endian
            }


            GameBaseLocation = DefaultPaths.SteamLegendaryEdition;
            WorkingLocation = DefaultPaths.WorkingDirectory;


            Game.Add(Games.Me1, new GameConfiguration
            {
                Name = "Mass Effect 1",
                CoalescedConfigurationLocation =
                    Path.Join(GameBaseLocation, @"ME1\BioGame\CookedPCConsole", "Coalesced_INT.bin"),
            });

            Game.Add(Games.Me2, new GameConfiguration
            {
                Name = "Mass Effect 2",
                CoalescedConfigurationLocation =
                    Path.Join(GameBaseLocation, @"ME2\BioGame\CookedPCConsole", "Coalesced_INT.bin"),
            });

            Game.Add(Games.Me3, new GameConfiguration
            {
                Name = "Mass Effect 3",
                CoalescedConfigurationLocation =
                    Path.Join(GameBaseLocation, @"ME3\BioGame\CookedPCConsole", "Coalesced.bin"),
            });


            _ = Directory.CreateDirectory(WorkingLocation);
        }


        #region Singleton

        // ReSharper disable once InconsistentNaming
        private static readonly MassEffectConfiguration _instance = new MassEffectConfiguration();

        static MassEffectConfiguration() { }

        private MassEffectConfiguration() { }

        // ReSharper disable once ConvertToAutoProperty
        // ReSharper disable once ReplaceAutoPropertyWithComputedProperty
        public static MassEffectConfiguration Instance { get; } = _instance;

        #endregion
    }
}