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
using System.IO;

namespace Aaron.MassEffect.Core
{
    internal static class DefaultPaths
    {
        // ReSharper disable once StringLiteralTypo
        public static string SteamLegendaryEdition { get; } =
            @"C:\Program Files (x86)\Steam\steamapps\common\Mass Effect Legendary Edition\Game\";

        public static string WorkingDirectory { get; } =
            Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "aaron");
    }
}