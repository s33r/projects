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
using System.Runtime.Serialization;

namespace Aaron.MassEffect.Core.Exceptions
{
    public class GameNotSupportedException : Exception
    {
        public Games Game { get; set; }

        public GameNotSupportedException(Games game)
            : base($"This method does not support the game: {game}")
        {
            Game = game;
        }

        public GameNotSupportedException(Games game, string message)
            : base(message)
        {
            Game = game;
        }

        public GameNotSupportedException(Games game, string message, Exception innerException)
            : base(message, innerException)

        {
            Game = game;
        }

        protected GameNotSupportedException(Games game, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Game = game;
        }
    }
}