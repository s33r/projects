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
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Aaron.Factory.CommandLine.Data
{
    public class Recipe : IEquatable<Recipe>
    {
        public List<IPort> AllPorts { get; }
        [JsonIgnore] public double Efficency { get; set; }

        public InputPort Input1 { get; }
        public InputPort Input2 { get; }
        public InputPort Input3 { get; }
        public InputPort Input4 { get; }
        public InputPort Input5 { get; }
        public List<InputPort> InputPorts { get; }

        [JsonIgnore] public int Instances { get; set; }

        public string Name { get; set; }
        public OutputPort Output1 { get; }
        public OutputPort Output2 { get; }
        public OutputPort Output3 { get; }
        public OutputPort Output4 { get; }
        public OutputPort Output5 { get; }
        public List<OutputPort> OutputPorts { get; }
        public double Time { get; set; }

        public Recipe()
        {
            Instances = 1;
            Efficency = 1;

            Input1 = new InputPort(this);
            Input2 = new InputPort(this);
            Input3 = new InputPort(this);
            Input4 = new InputPort(this);
            Input5 = new InputPort(this);

            InputPorts = new List<InputPort>
            {
                Input1,
                Input2,
                Input3,
                Input4,
                Input5,
            };

            Output1 = new OutputPort(this);
            Output2 = new OutputPort(this);
            Output3 = new OutputPort(this);
            Output4 = new OutputPort(this);
            Output5 = new OutputPort(this);

            OutputPorts = new List<OutputPort>
            {
                Output1,
                Output2,
                Output3,
                Output4,
                Output5,
            };

            AllPorts = InputPorts.Concat<IPort>(OutputPorts).ToList();
        }

        public Recipe(Dictionary<string, string> values)
        {
            if (values is null) { throw new ArgumentNullException(nameof(values)); }

            Instances = 1;
            Efficency = 1;

            Time = double.Parse(values["RECIPE.TIME"], CultureInfo.InvariantCulture);
            Name = values["RECIPE.NAME"];

            Input1 = new InputPort(this, values["INPUT1.NAME"], values["INPUT1.QUANTITY"]);
            Input2 = new InputPort(this, values["INPUT2.NAME"], values["INPUT2.QUANTITY"]);
            Input3 = new InputPort(this, values["INPUT3.NAME"], values["INPUT3.QUANTITY"]);
            Input4 = new InputPort(this, values["INPUT4.NAME"], values["INPUT4.QUANTITY"]);
            Input5 = new InputPort(this, values["INPUT5.NAME"], values["INPUT5.QUANTITY"]);

            InputPorts = new List<InputPort>
            {
                Input1,
                Input2,
                Input3,
                Input4,
                Input5,
            };

            Output1 = new OutputPort(this, values["OUTPUT1.NAME"], values["OUTPUT1.QUANTITY"]);
            Output2 = new OutputPort(this, values["OUTPUT2.NAME"], values["OUTPUT2.QUANTITY"]);
            Output3 = new OutputPort(this, values["OUTPUT3.NAME"], values["OUTPUT3.QUANTITY"]);
            Output4 = new OutputPort(this, values["OUTPUT4.NAME"], values["OUTPUT4.QUANTITY"]);
            Output5 = new OutputPort(this, values["OUTPUT5.NAME"], values["OUTPUT5.QUANTITY"]);

            OutputPorts = new List<OutputPort>
            {
                Output1,
                Output2,
                Output3,
                Output4,
                Output5,
            };

            AllPorts = InputPorts.Concat<IPort>(OutputPorts).ToList();
        }

        public Recipe(Recipe otherRecipe)
        {
            if (otherRecipe is null) { throw new ArgumentNullException(nameof(otherRecipe)); }

            Instances = otherRecipe.Instances;
            Efficency = otherRecipe.Efficency;

            Time = otherRecipe.Time;
            Name = otherRecipe.Name;

            Input1 = new InputPort(this, otherRecipe.Input1);
            Input2 = new InputPort(this, otherRecipe.Input2);
            Input3 = new InputPort(this, otherRecipe.Input3);
            Input4 = new InputPort(this, otherRecipe.Input4);
            Input5 = new InputPort(this, otherRecipe.Input5);

            InputPorts = new List<InputPort>
            {
                Input1,
                Input2,
                Input3,
                Input4,
                Input5,
            };

            Output1 = new OutputPort(this, otherRecipe.Output1);
            Output2 = new OutputPort(this, otherRecipe.Output2);
            Output3 = new OutputPort(this, otherRecipe.Output3);
            Output4 = new OutputPort(this, otherRecipe.Output4);
            Output5 = new OutputPort(this, otherRecipe.Output5);

            OutputPorts = new List<OutputPort>
            {
                Output1,
                Output2,
                Output3,
                Output4,
                Output5,
            };

            AllPorts = InputPorts.Concat<IPort>(OutputPorts).ToList();
        }

        public bool Equals(Recipe other)
        {
            if (other is null) { return false; }

            if (ReferenceEquals(this, other)) { return true; }

            return MatchName(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) { return false; }

            if (ReferenceEquals(this, obj)) { return true; }

            if (obj.GetType() != GetType()) { return false; }

            return Equals((Recipe)obj);
        }

        public InputPort FindInput(string name)
        {
            return FindPort(name, InputPorts);
        }

        public OutputPort FindOutput(string name)
        {
            return FindPort(name, OutputPorts);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool HasInput(string name)
        {
            return FindInput(name) is not null;
        }

        public bool HasOutput(string name)
        {
            return FindOutput(name) is not null;
        }

        public bool MatchName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }

            return CleanName(Name) == CleanName(name);
        }

        public static bool operator ==(Recipe left, Recipe right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Recipe left, Recipe right)
        {
            return !Equals(left, right);
        }

        public bool ShouldSerializeInput1()
        {
            return Input1.Active;
        }

        public bool ShouldSerializeInput2()
        {
            return Input2.Active;
        }

        public bool ShouldSerializeInput3()
        {
            return Input3.Active;
        }

        public bool ShouldSerializeInput4()
        {
            return Input4.Active;
        }

        public bool ShouldSerializeOutput1()
        {
            return Output1.Active;
        }

        public bool ShouldSerializeOutput2()
        {
            return Output2.Active;
        }

        public bool ShouldSerializeOutput3()
        {
            return Output3.Active;
        }

        public bool ShouldSerializeOutput4()
        {
            return Output4.Active;
        }

        public override string ToString()
        {
            return $"{Name} ({Time}s) x{Instances}";
        }

        private static string CleanName(string name)
        {
            return name.Trim().ToUpperInvariant();
        }

        private static T FindPort<T>(string name, IEnumerable<T> ports)
            where T : IPort
        {
            if (name == null) { return default(T); }

            string cleanName = CleanName(name);

            if (string.IsNullOrEmpty(cleanName)) { return default(T); }

            return ports.FirstOrDefault(port => port.Active && (CleanName(port.Name) == cleanName));
        }
    }
}