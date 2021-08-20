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
using System.IO;
using System.Linq;
using Aaron.Core.TextFiles.Csv;
using Newtonsoft.Json;

namespace Aaron.Factory.CommandLine.Data
{
    public enum RecipeTableFormat
    {
        Json,
        Csv,
    }

    public class RecipeTable
    {
        private readonly Dictionary<string, Recipe> _recipes;


        public RecipeTable()
        {
            _recipes = new Dictionary<string, Recipe>();
        }


        public IEnumerable<Recipe> FindInput(string name)
        {
            return _recipes.Values
                           .Where(recipe => recipe.HasInput(name))
                           .Select(recipe => new Recipe(recipe));
        }

        public IEnumerable<Recipe> FindOutput(string name)
        {
            return _recipes.Values
                           .Where(recipe => recipe.HasOutput(name))
                           .Select(recipe => new Recipe(recipe));
        }

        public void Load()
        {
            Load("./Data/recipes.json");
        }

        public void Load(string fileLocation)
        {
            Load(fileLocation, RecipeTableFormat.Json);
        }

        public void Load(string fileLocation, RecipeTableFormat format)
        {
            switch (format)
            {
                case RecipeTableFormat.Csv:
                    LoadCsv(fileLocation);
                    return;
                case RecipeTableFormat.Json:
                default:
                    string text = File.ReadAllText(fileLocation);
                    JsonConvert.DeserializeObject<List<Recipe>>(text).ForEach(r => _recipes.Add(r.Name, r));
                    return;
            }
        }

        public void Save()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            Recipe[] valueArray = new Recipe[_recipes.Count];
            _recipes.Values.CopyTo(valueArray, 0);

            string data = JsonConvert.SerializeObject(valueArray, settings);

            File.WriteAllText("./recipes.json", data);
        }

        private void LoadCsv(string fileLocation)
        {
            ParseResult result = Parser.Parse(fileLocation);

            result.ToDictionary()
                  .Select(row => new Recipe(row))
                  .ToList()
                  .ForEach(r => _recipes.Add(r.Name, r));
        }
    }
}