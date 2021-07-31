using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Core.CommandLine.Tokens
{
    public class BreakToken : IToken
    {
        public TokenTypes TokenType => TokenTypes.Break;
        public string Name { get; private set; }
        public string Value { get; private set; }
        public IToken Clean()
        {
            return this;
        }

        public IToken Collapse(IToken lookAhead, out bool consumed, out bool continueLook)
        {
            Value ??= "";

            if (lookAhead == null)
            {
                Value = "true";
                consumed = false;
                continueLook = false;
            }
            else
            {
                Value += $" {lookAhead.Name}";

                consumed = true;
                continueLook = true;
            }

            return this;
        }

        public IToken Convert()
        {
            return this;
        }

        public static bool Match(string name)
        {
            return (name.StartsWith('-') || name.StartsWith("--")) && name.Length == 2;
        }

        public BreakToken(string name)
        {
            if (Match(name))
            {
                Name = name;
            }
            else
            {
                throw new Exception("Invalid BreakToken");
            }
        }

        public override string ToString()
        {
            return $"[{TokenType}] {Name} = {Value}";
        }
    }
}
