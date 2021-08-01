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

using Aaron.MassEffect.Coalesced.Records;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced
{
    public enum EditorTypes
    {
        None,
        Checkbox,
        TextInput,
        MultilineInput,
        Integer,
        Decimal,
    }

    public class Annotation
    {
        private EditorTypes? EditorType { get; set; }
        public string EntryName { get; set; }
        public string FileName { get; set; }
        public Games? Game { get; set; }
        public string LongDescription { get; set; }
        public string SectionName { get; set; }
        public string ShortDescription { get; set; }

        public Annotation(Games game)
        {
            Game = game;
            EditorType = EditorTypes.None;
        }

        public Annotation()
            : this(Games.Unknown) { }

        public Annotation(Games game, FileRecord fileRecord)
            : this(game)
        {
            Game = game;
            FileName = fileRecord.Name;
        }

        public Annotation(FileRecord fileRecord)
            : this(Games.Unknown, fileRecord) { }

        public Annotation(Games game, SectionRecord sectionRecord)
            : this(game)
        {
            FileName = sectionRecord.Parent.Name;
            SectionName = sectionRecord.Name;
        }

        public Annotation(SectionRecord sectionRecord)
            : this(Games.Unknown, sectionRecord) { }

        public Annotation(Games game, EntryRecord entryRecord)
            : this(game)
        {
            Game = game;
            FileName = entryRecord.Parent.Parent.Name;
            SectionName = entryRecord.Parent.Name;
            EntryName = entryRecord.Name;
        }

        public Annotation(EntryRecord entryRecord)
            : this(Games.Unknown, entryRecord) { }


        public Annotation GetWireVersion()
        {
            return new Annotation
            {
                Game = null,
                FileName = string.IsNullOrEmpty(FileName)
                    ? null
                    : FileName,
                SectionName = string.IsNullOrEmpty(SectionName)
                    ? null
                    : SectionName,
                EntryName = string.IsNullOrEmpty(EntryName)
                    ? null
                    : EntryName,
                EditorType = EditorType == EditorTypes.None
                    ? null
                    : EditorType,
                ShortDescription = string.IsNullOrEmpty(ShortDescription)
                    ? null
                    : ShortDescription,
                LongDescription = string.IsNullOrEmpty(LongDescription)
                    ? null
                    : LongDescription,
            };
        }
    }
}