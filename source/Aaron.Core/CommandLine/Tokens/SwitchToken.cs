using System;

namespace Aaron.Core.CommandLine.Tokens
{
    public class SwitchToken : IToken
    {
        public SwitchToken(string name)
        {
            if (Match(name)) { Name = name; }
            else { throw new Exception("Invalid SwitchToken"); }
        }

        public IToken Clean()
        {
            Name = Name.Substring(1);

            return this;
        }

        public IToken Collapse(IToken lookAhead, out bool consumed, out bool continueLook)
        {
            consumed = false;
            continueLook = false;

            return this;
        }

        public IToken Convert()
        {
            return new ParameterToken(Name, "true");
        }

        public string Name { get; private set; }
        public TokenTypes TokenType => TokenTypes.Switch;
        public string Value { get; private set; }

        public static bool Match(string name)
        {
            return name.StartsWith('/');
        }

        public override string ToString()
        {
            return $"[{TokenType}] {Name} = {Value}";
        }
    }
}