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

namespace Aaron.Core.CommandLine.Syntax
{
    public class Parameter
    {
        public string Alias { get; set; }
        public string DefaultValue { get; set; }
        public string LongDescription { get; set; }
        public string Name { get; set; }
        public List<string> OptionDescription { get; }
        public List<string> Options { get; }
        public bool Required { get; set; }
        public string ShortDescription { get; set; }
        public string Value { get; set; }

        public Parameter(Parameter other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            Name = other.Name;
            Alias = other.Alias;
            Value = other.Value;
            Required = other.Required;
            DefaultValue = other.DefaultValue;
            Options = new List<string>(other.Options);
            OptionDescription = new List<string>(other.OptionDescription);
            ShortDescription = other.ShortDescription;
            LongDescription = other.LongDescription;
        }

        public Parameter(string name, string alias, IEnumerable<string> options, bool required, string defaultValue)
        {
            if (options == null) { throw new ArgumentNullException(nameof(options)); }

            Name = name;
            Alias = alias;
            Required = required;
            DefaultValue = defaultValue;

            ShortDescription = string.Empty;
            LongDescription = string.Empty;

            OptionDescription = new List<string>();
            Options = new List<string>();
            foreach (string option in options)
            {
                Options.Add(option);
                OptionDescription.Add(option);
            }

            if (!string.IsNullOrEmpty(DefaultValue)) { Value = DefaultValue; }
        }

        public Parameter(string name, string alias, IEnumerable<string> options, bool required)
            : this(name, alias, options, required, string.Empty) { }

        public Parameter(string name, IEnumerable<string> options, bool required, string defaultValue)
            : this(name, string.Empty, options, required, defaultValue) { }

        public Parameter(string name, bool required, string defaultValue)
            : this(name, string.Empty, Array.Empty<string>(), required, defaultValue) { }

        public Parameter(string name, string alias, IEnumerable<string> options)
            : this(name, alias, options, false, string.Empty) { }

        public Parameter(string name, IEnumerable<string> options)
            : this(name, string.Empty, options, false, string.Empty) { }

        public Parameter(string name, IEnumerable<string> options, bool required)
            : this(name, string.Empty, options, required, string.Empty) { }

        public Parameter(string name, bool required)
            : this(name, string.Empty, Array.Empty<string>(), required, string.Empty) { }

        public Parameter(string name, string alias)
            : this(name, alias, Array.Empty<string>(), false, string.Empty) { }

        public Parameter(string name)
            : this(name, string.Empty, Array.Empty<string>(), false, string.Empty) { }

        public Parameter()
            : this(string.Empty, string.Empty, Array.Empty<string>(), false, string.Empty) { }


        public override string ToString()
        {
            return $"Parameter: {Name} - {ShortDescription}";
        }
    }
}