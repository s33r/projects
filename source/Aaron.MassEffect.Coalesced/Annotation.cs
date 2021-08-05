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

        public Annotation(Games game, FileRecordCollection fileRecordCollection)
            : this(game)
        {
            if (fileRecordCollection == null) { throw new ArgumentNullException(nameof(fileRecordCollection)); }

            Game = game;
            FileName = fileRecordCollection.Name;
        }

        public Annotation(FileRecordCollection fileRecordCollection)
            : this(Games.Unknown, fileRecordCollection) { }

        public Annotation(Games game, SectionRecordCollection sectionRecordCollection)
            : this(game)
        {
            if (sectionRecordCollection == null) { throw new ArgumentNullException(nameof(sectionRecordCollection)); }

            FileName = sectionRecordCollection.Parent.Name;
            SectionName = sectionRecordCollection.Name;
        }

        public Annotation(SectionRecordCollection sectionRecordCollection)
            : this(Games.Unknown, sectionRecordCollection) { }

        public Annotation(Games game, EntryRecordCollection entryRecordCollection)
            : this(game)
        {
            if (entryRecordCollection == null) { throw new ArgumentNullException(nameof(entryRecordCollection)); }

            Game = game;
            FileName = entryRecordCollection.Parent.Parent.Name;
            SectionName = entryRecordCollection.Parent.Name;
            EntryName = entryRecordCollection.Name;
        }

        public Annotation(EntryRecordCollection entryRecordCollection)
            : this(Games.Unknown, entryRecordCollection) { }


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