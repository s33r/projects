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
using System.Linq;
using Aaron.Factory.CommandLine.Data;

namespace Aaron.Factory.CommandLine
{
    internal class Program
    {
        private static void Main()
        {
            RecipeTable table = new RecipeTable();
            table.Load("./Data/recipes.csv", RecipeTableFormat.Csv);
            table.Save();

            ProductionTargetCollection targets = new ProductionTargetCollection
            {
                new ProductionTarget("Sorter Mk. 3", table, 1),
                new ProductionTarget("Conveyer Belt Mk. 3", table, 1),
                new ProductionTarget("Assembling Machine Mk. 2", table, 1),
                new ProductionTarget("Mining Machine", table, 1),
                new ProductionTarget("Arc Smelter", table, 1),
                new ProductionTarget("Splitter", table, 1),
                new ProductionTarget("Tesla Tower", table, 1),
                new ProductionTarget("Foundation", table, 1),
                new ProductionTarget("Storage Mk. 1", table, 1),
                new ProductionTarget("Storage Mk. 2", table, 1),
                new ProductionTarget("Storage Tank", table, 1),
                new ProductionTarget("Solar Panel", table, 1),
            }.BuildTables(table).Flatten();


            List<Recipe> recipeList = targets.GetRecipes().Where(r => !r.IsTerminal).ToList();

            ReportMaker.CreateReport("./report.md", recipeList);
        }
    }
}