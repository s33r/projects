using System.Collections.Generic;
using System.Linq;

namespace Aaron.Core.CommandLine.Tokens
{
    public static class Tokenizer
    {
        private static IToken CreateToken(string name)
        {
            if (BreakToken.Match(name))
            {
                return new BreakToken(name);
            }

            if (SwitchToken.Match(name))
            {
                return new SwitchToken(name);
            }

            if (ParameterToken.Match(name))
            {
                return new ParameterToken(name);
            }

            return new ValueToken(name);
        }

        private static IEnumerable<IToken> CollapseTokens(IEnumerable<IToken> tokens)
        {
            List<IToken> result = new();
            Queue<IToken> tokenQueue = new(tokens);

            while (tokenQueue.Count > 0)
            {
                IToken token = tokenQueue.Dequeue();
                bool continueLook = true;

                while (continueLook)
                {
                    IToken nextToken = tokenQueue.Count > 0 ? tokenQueue.Peek() : null;

                    token = token.Collapse(nextToken, out bool consumed, out bool outContinueLook);
                    continueLook = outContinueLook;

                    if (consumed)
                    {
                        tokenQueue.Dequeue();
                    }
                }

                result.Add(token);
            }

            return result;
        }

        public static List<IToken> Parse(string[] args, List<CommandLineError> errors)
        {
            if (args == null || args.Length == 0)
            {
                errors.Add(new CommandLineError()
                {
                    Message = "The are no arguments to tokenize.",
                });

                return new List<IToken>();
            }

            List<IToken> tokens = args
                .Select(CreateToken)
                .Select(t => t.Clean())
                .ToList();

            tokens = CollapseTokens(tokens)
                .Select(t => t.Convert())
                .ToList();

            return tokens;
        }
    }
}
