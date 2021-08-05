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
    public class SectionRecordCollection
        : IRecordCollection, IEquatable<IRecordCollection>, IList<EntryRecordCollection>
    {
        private List<EntryRecordCollection> _values;

        public SectionRecordCollection(List<EntryRecordCollection> entries, string name)
        {
            if (entries == null) { throw new ArgumentNullException(nameof(entries)); }

            Name = name;
            _values = new List<EntryRecordCollection>();

            foreach (EntryRecordCollection entryRecord in entries)
            {
                entryRecord.Parent = this;
                _values.Add(entryRecord);
            }
        }

        public SectionRecordCollection(List<EntryRecordCollection> sections)
            : this(sections, null) { }

        public SectionRecordCollection(int count)
            : this(Utility.CreateList<EntryRecordCollection>(count).ToList()) { }

        public SectionRecordCollection()
            : this(new List<EntryRecordCollection>()) { }

        public SectionRecordCollection(string name)
            : this(new List<EntryRecordCollection>(), name) { }

        public void Add(EntryRecordCollection item)
        {
            if (item == null) { throw new ArgumentNullException(nameof(item)); }

            item.Parent = this;
            _values.Add(item);
        }

        public void Clear()
        {
            foreach (EntryRecordCollection item in _values) { item.Parent = null; }

            _values.Clear();
        }

        public bool Contains(EntryRecordCollection item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(EntryRecordCollection[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public int Count => _values.Count;

        public bool Equals(IRecordCollection other)
        {
            if (other == null) { return false; }

            return other.Name == Name;
        }

        public IEnumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator<EntryRecordCollection> IEnumerable<EntryRecordCollection>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public int IndexOf(EntryRecordCollection item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, EntryRecordCollection item)
        {
            if (item == null) { throw new ArgumentNullException(nameof(item)); }

            item.Parent = this;
            _values.Insert(index, item);
        }

        public bool IsReadOnly => false;

        public EntryRecordCollection this[int index]
        {
            get => _values[index];
            set
            {
                if (value == null) { throw new ArgumentNullException(nameof(value)); }

                value.Parent = this;
                _values[index] = value;
            }
        }

        public string Name { get; set; }

        public IRecordCollection Parent { get; internal set; }

        public string Path => Parent.Name + '/' + Name;

        public bool Remove(EntryRecordCollection item)
        {
            if (item == null) { throw new ArgumentNullException(nameof(item)); }

            item.Parent = null;
            return _values.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _values[index].Parent = null;
            _values.RemoveAt(index);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) { return false; }

            return Equals((IRecordCollection)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }


        public void SetValues(IEnumerable<EntryRecordCollection> entries)
        {
            if (entries == null) { throw new ArgumentNullException(nameof(entries)); }

            _values = new List<EntryRecordCollection>();

            foreach (EntryRecordCollection entry in entries)
            {
                entry.Parent = this;
                _values.Add(entry);
            }
        }

        public void Sort(Comparison<IRecordCollection> comparer)
        {
            _values.Sort(comparer);
        }

        public override string ToString()
        {
            return $"SectionRecord [{_values.Count} Values] {Name}";
        }
    }
}