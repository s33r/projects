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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aaron.Factory.CommandLine.Data;

namespace Aaron.Factory.CommandLine
{
    public class ProductionTargetCollection : ICollection<ProductionTarget>
    {
        private readonly Dictionary<string, ProductionTarget> _targets = new Dictionary<string, ProductionTarget>();

        /// <summary>
        ///     Adds a production target to the collection. If the recipe is already in the collection, the number of instances
        ///     will be added to that recipe.
        /// </summary>
        /// <param name="item">The ProductionTarget to add</param>
        public void Add(ProductionTarget item)
        {
            if (item is null) { throw new ArgumentNullException(nameof(item)); }

            if (_targets.ContainsKey(item.Recipe.Name))
            {
                _targets[item.Recipe.Name].Recipe.Instances += item.Recipe.Instances;
            }
            else { _targets.Add(item.Recipe.Name, item); }
        }

        public void Clear()
        {
            _targets.Clear();
        }

        public bool Contains(ProductionTarget item)
        {
            return Contains(item?.Recipe.Name ?? null);
        }

        public void CopyTo(ProductionTarget[] array, int arrayIndex)
        {
            _targets.Values.CopyTo(array, arrayIndex);
        }

        public int Count => _targets.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ProductionTarget> GetEnumerator()
        {
            return _targets.Values.GetEnumerator();
        }

        public bool IsReadOnly { get; }

        public bool Remove(ProductionTarget item)
        {
            return Remove(item?.Recipe.Name ?? null);
        }

        public ProductionTargetCollection BuildTables(RecipeTable table)
        {
            foreach (ProductionTarget productionTarget in _targets.Values) { productionTarget.BuildTable(table); }

            return this;
        }

        public bool Contains(string recipeName)
        {
            if (string.IsNullOrEmpty(recipeName)) { return false; }

            return _targets.ContainsKey(recipeName);
        }

        public ProductionTargetCollection Flatten()
        {
            ProductionTargetCollection result = new ProductionTargetCollection();

            foreach (ProductionTarget productionTarget in _targets.Values)
            {
                foreach (Recipe recipe in productionTarget.GetRecipes(false, TargetSortMode.Instances))
                {
                    result.Add(new ProductionTarget(recipe));
                }
            }

            return result;
        }

        public IEnumerable<Recipe> GetRecipes(TargetSortMode sortMode = TargetSortMode.Instances)
        {
            IEnumerable<ProductionTarget> targets = _targets.Values;

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

        public bool Remove(string recipeName)
        {
            if (!_targets.ContainsKey(recipeName)) { return false; }

            return _targets.Remove(recipeName);
        }
    }
}