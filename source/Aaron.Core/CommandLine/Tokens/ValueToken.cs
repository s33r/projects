using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Core.CommandLine.Tokens
{
    public class ValueToken : IToken
    {
        public TokenTypes TokenType => TokenTypes.Value;
        public string Name { get; private set; }
        public string Value { get; private set; }
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

        public ValueToken(string value)
        {
            Name = value;
        }

        public override string ToString()
        {
            return $"[{TokenType}] {Name} = {Value}";
        }

    }
}
