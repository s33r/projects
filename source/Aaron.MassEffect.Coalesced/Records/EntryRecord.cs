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

namespace Aaron.MassEffect.Coalesced.Records
{
    public class EntryRecord
        : IRecord, IEquatable<IRecord>, IList<string>
    {
        private List<string> _values;

        public EntryRecord(List<string> items, string name)
        {
            Name = name;
            _values = new List<string>(items);
        }

        public EntryRecord(List<string> items)
            : this(items, null) { }

        public EntryRecord()
            : this(new List<string>()) { }

        public EntryRecord(string name)
            : this(new List<string>(), name) { }

        public void Add(string item)
        {
            _values.Add(item);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(string item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public int Count => _values.Count;

        public bool Equals(IRecord other)
        {
            if (other == null) { return false; }

            return other.Name == Name;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            _values.Insert(index, item);
        }

        public bool IsReadOnly => false;

        public string this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public string Name { get; set; }

        public IRecord Parent { get; internal set; }

        public string Path => Parent.Parent.Name + '/' + Parent.Name + '/' + Name;

        public bool Remove(string item)
        {
            return _values.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _values.RemoveAt(index);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) { return false; }

            return Equals((IRecord)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }


        public void SetValues(IEnumerable<string> items)
        {
            _values = new List<string>(items);
        }

        public override string ToString()
        {
            return $"EntryRecord [{_values.Count} Values] {Name}";
        }
    }
}