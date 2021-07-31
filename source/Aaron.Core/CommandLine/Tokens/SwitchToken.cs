using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Core.CommandLine.Tokens
{
    public class SwitchToken : IToken
    {
        public TokenTypes TokenType => TokenTypes.Switch;
        public string Name { get; private set; }
        public string Value { get; private set; }
        public IToken Clean()
        {
            Name = Name.Substring(1);

            return this;
        }

        public IToken Convert()
        {
            return new ParameterToken(Name, "true");
        }

        public IToken Collapse(IToken lookAhead, out bool consumed, out bool continueLook)
        {
            consumed = false;
            continueLook = false;

            return this;
        }

        public SwitchToken(string name)
        {
            if (Match(name))
            {
                Name = name;
            }
            else
            {
                throw new Exception("Invalid SwitchToken");
            }
        }

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
