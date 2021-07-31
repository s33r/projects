using System.Collections.Generic;

namespace Aaron.Core.CommandLine.Tokens
{

    public enum TokenTypes
    {
        Unknown,
        Parameter,
        Switch,
        Value,
        Break,
    }

    public interface IToken
    {
        TokenTypes TokenType { get; }

        string Name { get; }
        string Value { get; }
        IToken Clean();
        IToken Collapse(IToken lookAhead, out bool consumed, out bool continueLook);
        IToken Convert();
    }
}
