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

namespace Aaron.Core.CommandLine.Tokens
{
    public class BreakToken : IToken
    {
        public BreakToken(string name)
        {
            if (Match(name)) { Name = name; }
            else { throw new Exception("Invalid BreakToken"); }
        }

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

        public string Name { get; }
        public TokenTypes TokenType => TokenTypes.Break;
        public string Value { get; private set; }

        public static bool Match(string name)
        {
            return (name.StartsWith('-') || name.StartsWith("--")) && name.Length == 2;
        }

        public override string ToString()
        {
            return $"[{TokenType}] {Name} = {Value}";
        }
    }
}