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
using System.Globalization;
using Newtonsoft.Json;

namespace Aaron.Factory.CommandLine.Data
{
    public class InputPort : IPort
    {
        [JsonIgnore] public double Demand => Parent.Instances * (Quantity / Parent.Time);

        public InputPort(Recipe parent)
        {
            if (parent is null) { throw new ArgumentNullException(nameof(parent)); }

            Parent = parent;
        }

        public InputPort(Recipe parent, string name, string quantity)
            : this(parent)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
                Quantity = int.Parse(quantity, CultureInfo.InvariantCulture);
                Active = true;
            }
        }

        public InputPort(Recipe parent, InputPort otherPort)
        {
            if (parent is null) { throw new ArgumentNullException(nameof(parent)); }

            if (otherPort is null) { throw new ArgumentNullException(nameof(otherPort)); }

            Parent = parent;
            Active = otherPort.Active;
            Quantity = otherPort.Quantity;
            Name = otherPort.Name;
        }

        public bool Active { get; set; }
        public string Name { get; set; }

        [JsonIgnore] public Recipe Parent { get; }

        public int Quantity { get; set; }

        public override string ToString()
        {
            if (!Active) { return "Input Disabled"; }

            return $"{Name} @ {Demand}/s";
        }
    }
}