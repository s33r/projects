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
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Core.CommandLine
{
    public class ParameterBuilder
    {
        private readonly Dictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();

        public ParameterBuilder() { }

        public ParameterBuilder(ParameterBuilder other)
        {
            if (other == null) { return; }

            List<Parameter> otherCommands = other.ToList();

            foreach (Parameter parameter in otherCommands) { _ = AddParameter(new Parameter(parameter)); }
        }

        public ParameterBuilder AddParameter(Parameter parameter)
        {
            if (parameter == null) { throw new ArgumentNullException(nameof(parameter)); }

            if (string.IsNullOrEmpty(parameter.Name))
            {
                throw new ArgumentException("The name of the parameter cannot be null.");
            }

            if (_parameters.ContainsKey(parameter.Name)) { throw new DuplicateParameterException(parameter.Name); }

            _parameters.Add(parameter.Name, parameter);

            return this;
        }

        public bool HasParameter(string name)
        {
            return _parameters.ContainsKey(name);
        }

        public void SetValue(string name, string value)
        {
            _parameters[name].Value = value;
        }

        public Dictionary<string, Parameter> ToDictionary()
        {
            Dictionary<string, Parameter> result = new Dictionary<string, Parameter>();

            foreach (KeyValuePair<string, Parameter> keyValuePair in _parameters)
            {
                result.Add(keyValuePair.Key, new Parameter(keyValuePair.Value));
            }

            return result;
        }

        public List<Parameter> ToList()
        {
            return new List<Parameter>(_parameters.Values);
        }
    }
}