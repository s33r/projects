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

namespace Aaron.MassEffect.Core
{
    public static class Utility
    {
        public static IEnumerable<T> CreateList<T>(int count)
            where T : new()
        {
            List<T> result = new List<T>(count);

            PopulateList(count, result);

            return result;
        }

        public static void PopulateList<T>(int count, List<T> list)
            where T : new()
        {
            for (int j = 0; j < count; j++) { list.Add(new T()); }
        }
    }
}