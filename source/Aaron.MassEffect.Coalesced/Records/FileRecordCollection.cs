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
    public class FileRecordCollection
        : IRecordCollection, IEquatable<IRecordCollection>, IList<SectionRecordCollection>
    {
        private List<SectionRecordCollection> _values;

        //Windows can use either slash, but all others need unix style.
        public string FriendlyName =>
            System.IO.Path.GetFileName(Name.Replace("\\", "/"));

        public FileRecordCollection(List<SectionRecordCollection> sections, string name)
        {
            if (sections == null) { throw new ArgumentNullException(nameof(sections)); }

            Name = name;
            _values = new List<SectionRecordCollection>();

            foreach (SectionRecordCollection sectionRecord in sections)
            {
                sectionRecord.Parent = this;
                _values.Add(sectionRecord);
            }
        }

        public FileRecordCollection(List<SectionRecordCollection> sections)
            : this(sections, null) { }

        public FileRecordCollection(int count)
            : this(Utility.CreateList<SectionRecordCollection>(count).ToList()) { }

        public FileRecordCollection()
            : this(new List<SectionRecordCollection>()) { }

        public FileRecordCollection(string name)
            : this(new List<SectionRecordCollection>(), name) { }

        public void Add(SectionRecordCollection item)
        {
            if (item == null) { throw new ArgumentNullException(nameof(item)); }

            item.Parent = this;
            _values.Add(item);
        }

        public void Clear()
        {
            foreach (SectionRecordCollection item in _values) { item.Parent = null; }

            _values.Clear();
        }

        public bool Contains(SectionRecordCollection item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(SectionRecordCollection[] array, int arrayIndex)
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

        IEnumerator<SectionRecordCollection> IEnumerable<SectionRecordCollection>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public int IndexOf(SectionRecordCollection item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, SectionRecordCollection item)
        {
            if (item == null) { throw new ArgumentNullException(nameof(item)); }

            item.Parent = this;
            _values.Insert(index, item);
        }

        public bool IsReadOnly => false;

        public SectionRecordCollection this[int index]
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

        public IRecordCollection Parent => null;
        public string Path => FriendlyName;

        public bool Remove(SectionRecordCollection item)
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

        public void SetValues(IEnumerable<SectionRecordCollection> sections)
        {
            if (sections == null) { throw new ArgumentNullException(nameof(sections)); }

            _values = new List<SectionRecordCollection>();

            foreach (SectionRecordCollection section in sections)
            {
                section.Parent = this;
                _values.Add(section);
            }
        }

        public void Sort(Comparison<IRecordCollection> comparer)
        {
            _values.Sort(comparer);
        }

        public override string ToString()
        {
            return $"FileRecord [{_values.Count} Values] {Name}";
        }
    }
}