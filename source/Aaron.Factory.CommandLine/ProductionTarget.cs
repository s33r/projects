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
using Aaron.Factory.CommandLine.Data;

namespace Aaron.Factory.CommandLine
{
    public enum TargetSortMode
    {
        None,
        Alpha,
        Depth,
        Instances,
    }

    /// <summary>
    ///     Represents
    /// </summary>
    public class ProductionTarget
    {
        public List<ProductionTarget> Children { get; private set; }
        public int PathLength { get; private set; }
        public Recipe Recipe { get; }


        public ProductionTarget(Recipe recipe)
        {
            if (recipe is null) { throw new ArgumentNullException(nameof(recipe)); }

            Recipe = new Recipe(recipe);
        }

        public ProductionTarget(string item, RecipeTable table, int instances)
        {
            if (table is null) { throw new ArgumentNullException(nameof(table)); }

            Recipe = table.FindOutput(item).First();
            Recipe.Instances = instances;
        }

        public ProductionTarget(string item, RecipeTable table, double supply)
        {
            if (table is null) { throw new ArgumentNullException(nameof(table)); }

            Recipe = table.FindOutput(item).First();
            double baseSupply = Recipe.FindOutput(item).Supply;
            Recipe.Instances = (int)Math.Ceiling(supply / baseSupply);
        }

        public ProductionTarget(string item, RecipeTable table)
        {
            if (table is null) { throw new ArgumentNullException(nameof(table)); }

            Recipe = table.FindOutput(item).First();
        }


        public void BuildTable(RecipeTable table)
        {
            Children = new List<ProductionTarget>();

            foreach (InputPort inputPort in Recipe.InputPorts)
            {
                if (!inputPort.Active) { continue; }

                ProductionTarget nextTarget = new ProductionTarget(inputPort.Name, table, inputPort.Demand)
                {
                    PathLength = PathLength + 1,
                };

                Children.Add(nextTarget);
                nextTarget.BuildTable(table);
            }
        }

        public IEnumerable<ProductionTarget> Deduplicate()
        {
            return Deduplicate(EnumeratePreOrder());
        }

        public IEnumerable<ProductionTarget> EnumeratePostOrder()
        {
            foreach (ProductionTarget target in Children)
            {
                foreach (ProductionTarget child in target.EnumeratePreOrder()) { yield return child; }
            }

            yield return this;
        }

        public IEnumerable<ProductionTarget> EnumeratePreOrder()
        {
            yield return this;

            foreach (ProductionTarget target in Children)
            {
                foreach (ProductionTarget child in target.EnumeratePreOrder()) { yield return child; }
            }
        }

        public IEnumerable<Recipe> GetRecipes(bool deduplicate, TargetSortMode sortMode = TargetSortMode.Depth)
        {
            IEnumerable<ProductionTarget> targets = EnumeratePreOrder();

            if (deduplicate) { targets = Deduplicate(targets); }

            switch (sortMode)
            {
                case TargetSortMode.Depth:
                    targets = targets.OrderBy(target => target.PathLength);
                    break;
                case TargetSortMode.Alpha:
                    targets = targets.OrderBy(target => target.Recipe.Name);
                    break;
                case TargetSortMode.Instances:
                    targets = targets.OrderBy(target => target.Recipe.Instances);
                    break;
                case TargetSortMode.None:
                default:
                    break;
            }

            return targets.Select(target => target.Recipe);
        }

        private static IEnumerable<ProductionTarget> Deduplicate(IEnumerable<ProductionTarget> targets)
        {
            Dictionary<string, ProductionTarget> dedupTargets = new Dictionary<string, ProductionTarget>();

            foreach (ProductionTarget target in targets)
            {
                string key = target.Recipe.Name;

                if (!dedupTargets.ContainsKey(key)) { dedupTargets.Add(key, target); }
                else { dedupTargets[key].Recipe.Instances += target.Recipe.Instances; }
            }

            return dedupTargets.Values;
        }
    }
}