using System;
using System.Collections.Generic;

namespace Aaron.Core.CommandLine.Syntax
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public List<string> Options { get; set; }
        public List<string> OptionDescription { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public Parameter(Parameter other)
        {
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

            if (!string.IsNullOrEmpty(DefaultValue))
            {
                Value = DefaultValue;
            }
        }

        public Parameter(string name, string alias, IEnumerable<string> options, bool required) 
            :this(name, alias, options, required, string.Empty) { }

        public Parameter(string name, IEnumerable<string> options, bool required, string defaultValue) 
            :this(name, string.Empty, options, false, defaultValue) { }

        public Parameter(string name, bool required, string defaultValue)
            : this(name, string.Empty, Array.Empty<string>(), required, defaultValue) { }

        public Parameter(string name, string alias, IEnumerable<string> options) 
            :this(name, alias, options, false, string.Empty) { }

        public Parameter(string name, IEnumerable<string> options) 
            :this(name, string.Empty, options, false, string.Empty) { }

        public Parameter(string name, IEnumerable<string> options, bool required) 
            :this(name, string.Empty, options, false, string.Empty) { }

        public Parameter(string name, bool required)
            :this(name, string.Empty, Array.Empty<string>(), required, string.Empty) { }

        public Parameter(string name, string alias)
            :this(name, alias, Array.Empty<string>(), false, string.Empty) { }

        public Parameter(string name)
            : this(name, string.Empty, Array.Empty<string>(), false, string.Empty) { }

        public Parameter()
            : this(string.Empty, string.Empty, Array.Empty<string>(), false, string.Empty) { }






    }
}
