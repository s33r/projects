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
    public class SwitchToken : IToken
    {
        public SwitchToken(string name)
        {
            if (Match(name)) { Name = name; }
            else { throw new InvalidTokenException(TokenTypes.Switch, name); }
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
            if (name == null) { throw new ArgumentNullException(nameof(name)); }

            return name.StartsWith('/');
        }

        public override string ToString()
        {
            return $"[{TokenType}] {Name} = {Value}";
        }
    }
}