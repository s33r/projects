// Copyright (C) 2021 Aaron C. Willows (aaron@aaronwillows.com)
// 
// This program is free software; you can redistribute it and/or modify it under the terms of the
// GNU Lesser General Public License as published by the Free Software Foundation; either version
// 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
// the GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
// MA 02111-1307 USA

using System;
using System.Collections.Generic;
using System.Linq;

namespace Aaron.Core.CommandLine.Tokens
{
    public static class Tokenizer
    {
        public static List<IToken> Parse(string[] args, List<CommandLineError> errors)
        {
            if (errors == null) { throw new ArgumentNullException(nameof(errors)); }

            if (args == null || args.Length == 0)
            {
                errors.Add(new CommandLineError { Message = "The are no arguments to tokenize." });

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

        private static IEnumerable<IToken> CollapseTokens(IEnumerable<IToken> tokens)
        {
            List<IToken> result = new List<IToken>();
            Queue<IToken> tokenQueue = new Queue<IToken>(tokens);

            while (tokenQueue.Count > 0)
            {
                IToken token = tokenQueue.Dequeue();
                bool continueLook = true;

                while (continueLook)
                {
                    IToken nextToken = tokenQueue.Count > 0
                        ? tokenQueue.Peek()
                        : null;

                    token = token.Collapse(nextToken, out bool consumed, out bool outContinueLook);
                    continueLook = outContinueLook;

                    if (consumed) { _ = tokenQueue.Dequeue(); }
                }

                result.Add(token);
            }

            return result;
        }

        private static IToken CreateToken(string name)
        {
            if (BreakToken.Match(name)) { return new BreakToken(name); }

            if (SwitchToken.Match(name)) { return new SwitchToken(name); }

            if (ParameterToken.Match(name)) { return new ParameterToken(name); }

            return new ValueToken(name);
        }
    }
}