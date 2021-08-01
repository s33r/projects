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

            foreach (Parameter parameter in otherCommands) { AddParameter(new Parameter(parameter)); }
        }

        public ParameterBuilder AddParameter(Parameter parameter)
        {
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