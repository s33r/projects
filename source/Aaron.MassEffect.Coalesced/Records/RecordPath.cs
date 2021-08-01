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

using System.Text;

namespace Aaron.MassEffect.Coalesced.Records
{
    public class RecordPath
    {
        public const char PATH_SEPARATOR = '/';
        public string EntryName { get; set; }

        public string FileName { get; set; }
        public string SectionName { get; set; }

        public static RecordPath FromString(string path)
        {
            if (string.IsNullOrEmpty(path)) { return new RecordPath(); }

            string[] elements = path.Split(PATH_SEPARATOR);

            if (elements.Length == 0) { return new RecordPath(); }

            RecordPath result = new RecordPath() {EntryName = elements[0]};

            if (elements.Length > 1)
            {
                result.SectionName = elements[1];

                if (elements.Length > 2) { result.EntryName = elements[2]; }
            }


            return result;
        }

        public override string ToString()
        {
            if (FileName == null) { return string.Empty; }

            StringBuilder buffer = new StringBuilder();

            buffer.Append(FileName);

            if (SectionName != null)
            {
                buffer.Append(PATH_SEPARATOR + SectionName);

                if (EntryName != null) { buffer.Append(PATH_SEPARATOR + EntryName); }
            }

            return buffer.ToString();
        }
    }
}