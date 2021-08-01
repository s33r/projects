namespace Aaron.Core.CommandLine.Tokens
{
    public class ValueToken : IToken
    {
        public ValueToken(string value)
        {
            Name = value;
        }

        public IToken Clean()
        {
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

        public string Name { get; }
        public TokenTypes TokenType => TokenTypes.Value;
        public string Value { get; private set; }

        public override string ToString()
        {
            return $"[{TokenType}] {Name} = {Value}";
        }
    }
}