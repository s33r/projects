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
        string Name { get; }
        TokenTypes TokenType { get; }
        string Value { get; }
        IToken Clean();
        IToken Collapse(IToken lookAhead, out bool consumed, out bool continueLook);
        IToken Convert();
    }
}