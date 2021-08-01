using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Aaron.Core.Tests")]

namespace Aaron.Core.CommandLine.Tokens
{
    public class ParameterToken : IToken
    {
        public ParameterToken(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public ParameterToken(string name)
            : this(name, string.Empty) { }

        public ParameterToken() { }

        public IToken Clean()
        {
            int index;
            for (index = 0; index < Name.Length; index++)
            {
                if (Name[index] != '-') { break; }
            }

            Name = Name.Substring(index);

            return this;
        }

        public IToken Collapse(IToken lookAhead, out bool consumed, out bool continueLook)
        {
            if (lookAhead == null)
            {
                Value = "true";
                consumed = false;
            }
            else if (lookAhead.TokenType == TokenTypes.Value)
            {
                Value = lookAhead.Name;
                consumed = true;
            }
            else
            {
                Value = "true";
                consumed = false;
            }

            continueLook = false;

            return this;
        }

        public IToken Convert()
        {
            return this;
        }

        public string Name { get; private set; }
        public TokenTypes TokenType => TokenTypes.Parameter;
        public string Value { get; private set; }

        public static bool Match(string name)
        {
            return name.StartsWith('-') || name.StartsWith("--");
        }

        public override string ToString()
        {
            return $"[{TokenType}] {Name} = {Value}";
        }
    }
}