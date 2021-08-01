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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.Coalesced.Records
{
    public class FileRecord
        : IRecord, IEquatable<IRecord>, IList<SectionRecord>
    {
        private List<SectionRecord> _values;

        public string FriendlyName =>
            System.IO.Path.GetFileName(Name.Replace("\\",
                "/")); //Windows can use either slash, but all others need unix style.

        public FileRecord(List<SectionRecord> sections, string name)
        {
            Name = name;
            _values = new List<SectionRecord>();

            foreach (SectionRecord sectionRecord in sections)
            {
                sectionRecord.Parent = this;
                _values.Add(sectionRecord);
            }
        }

        public FileRecord(List<SectionRecord> sections)
            : this(sections, null) { }

        public FileRecord(int count)
            : this(Utility.CreateList<SectionRecord>(count).ToList()) { }

        public FileRecord()
            : this(new List<SectionRecord>()) { }

        public FileRecord(string name)
            : this(new List<SectionRecord>(), name) { }

        public void Add(SectionRecord item)
        {
            item.Parent = this;
            _values.Add(item);
        }

        public void Clear()
        {
            foreach (SectionRecord item in _values) { item.Parent = null; }

            _values.Clear();
        }

        public bool Contains(SectionRecord item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(SectionRecord[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public int Count => _values.Count;


        public bool Equals(IRecord other)
        {
            if (other == null) { return false; }

            return other.Name == Name;
        }

        public IEnumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator<SectionRecord> IEnumerable<SectionRecord>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public int IndexOf(SectionRecord item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, SectionRecord item)
        {
            item.Parent = this;
            _values.Insert(index, item);
        }

        public bool IsReadOnly => false;

        public SectionRecord this[int index]
        {
            get => _values[index];
            set
            {
                value.Parent = this;
                _values[index] = value;
            }
        }

        public string Name { get; set; }

        public IRecord Parent => null;
        public string Path => FriendlyName;

        public bool Remove(SectionRecord item)
        {
            item.Parent = null;
            return _values.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _values[index].Parent = null;
            _values.RemoveAt(index);
        }

        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType()) { return false; }

            return Equals((IRecord)other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public void SetValues(IEnumerable<SectionRecord> sections)
        {
            _values = new List<SectionRecord>();

            foreach (SectionRecord section in sections)
            {
                section.Parent = this;
                _values.Add(section);
            }
        }

        public void Sort(Comparison<IRecord> comparer)
        {
            _values.Sort(comparer);
        }

        public override string ToString()
        {
            return $"FileRecord [{_values.Count} Values] {Name}";
        }
    }
}