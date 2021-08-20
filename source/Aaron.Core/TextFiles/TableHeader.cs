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

namespace Aaron.Core.TextFiles
{
    public class TableHeader
    {
        public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Left;
        public string Format { get; set; }
        public bool HideIfEmpty { get; set; }

        internal bool IsEmpty { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }


        public TableHeader()
        {
            HideIfEmpty = true;
            IsEmpty = true;
        }

        public TableHeader(TableHeader header)
        {
            if (header is null) { throw new ArgumentNullException(nameof(header)); }

            Format = header.Format;
            Name = header.Name;
            Width = header.Width;
            Alignment = header.Alignment;
            HideIfEmpty = header.HideIfEmpty;
            IsEmpty = header.IsEmpty;
        }
    }
}