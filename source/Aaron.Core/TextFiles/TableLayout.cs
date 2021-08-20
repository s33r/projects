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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Aaron.Core.Extensions;

namespace Aaron.Core.TextFiles
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right,
    }

    public class TableLayout
    {
        private readonly List<TableHeader> _headers = new List<TableHeader>();
        private readonly List<List<string>> _rows = new List<List<string>>();

        public int ColumnCount => _headers.Count;
        public int RowCount => _rows.Count;

        public TableLayout AddHeader(TableHeader header)
        {
            _headers.Add(new TableHeader(header));

            return this;
        }

        public TableLayout AddHeader(string name)
        {
            _headers.Add(new TableHeader
            {
                Name = name,
            });

            return this;
        }

        public TableLayout AddHeader(string name, HorizontalAlignment alignment)
        {
            _headers.Add(new TableHeader
            {
                Name = name,
                Alignment = alignment,
            });

            return this;
        }


        public TableLayout AddRow(IEnumerable<string> columns)
        {
            if (columns is null) { return this; }

            List<string> row = columns.ToList();

            if (row.Count != _headers.Count)
            {
                throw new InvalidRowException(
                    $"There are {_headers.Count} columns in this table, but {row.Count} columns in this row");
            }

            _rows.Add(columns.ToList());

            return this;
        }

        public TableLayout AddRows(IEnumerable<IEnumerable<string>> rows)
        {
            if (rows is null) { return this; }

            foreach (IEnumerable<string> row in rows) { AddRow(row); }

            return this;
        }

        public IEnumerable<IEnumerable<string>> GetTable()
        {
            List<TableHeader> headers = _headers.Select(h => new TableHeader(h)).ToList();
            List<List<string>> result = new List<List<string>>();

            List<string> headerRow = new List<string>();
            foreach (TableHeader header in headers) { headerRow.Add(RenderHeader(header)); }

            result.Add(headerRow);

            foreach (List<string> row in _rows)
            {
                List<string> newRow = new List<string>();

                for (int i = 0; i < row.Count; i++) { newRow.Add(RenderValue(row[i], headers[i])); }

                result.Add(newRow);
            }

            foreach (List<string> row in result)
            {
                for (int i = 0; i < row.Count; i++) { row[i] = PadValue(row[i], headers[i]); }
            }

            for (int rowIndex = 0; rowIndex < result.Count; rowIndex++)
            {
                List<string> row = result[rowIndex];

                result[rowIndex] = RemoveColumns(row, headers);
            }

            foreach (List<string> row in result)
            {
                List<string> newRow = new List<string>();

                for (int i = 0; i < row.Count; i++) { row[i] = PadValue(row[i], headers[i]); }
            }

            return result;
        }

        private static string PadValue(string value, TableHeader header)
        {
            string formatedValue = value ?? string.Empty;

            switch (header.Alignment)
            {
                case HorizontalAlignment.Center:
                    //TODO: fix .Center to work consistantly with odd numbers
                    return formatedValue.Center(header.Width, " ");
                case HorizontalAlignment.Right:
                    return formatedValue.PadLeft(header.Width);
                    ;
                case HorizontalAlignment.Left:
                default:
                    return formatedValue.PadRight(header.Width);
            }
        }

        private static List<string> RemoveColumns(List<string> row, List<TableHeader> headers)
        {
            List<string> newRow = new List<string>();

            for (int i = 0; i < headers.Count; i++)
            {
                TableHeader header = headers[i];
                string value = row[i];
                bool skip = header.IsEmpty && header.HideIfEmpty;

                if (!skip) { newRow.Add(value); }
            }

            return newRow;
        }

        private static string RenderHeader(TableHeader header)
        {
            string value = header.Name;

            if (string.IsNullOrEmpty(value)) { return string.Empty; }

            if (header.Width < value.Length) { header.Width = value.Length; }

            return value;
        }

        private static string RenderValue(string value, TableHeader header)
        {
            if (string.IsNullOrEmpty(value)) { return string.Empty; }

            string formatedValue = value;

            if (header.Format != null)
            {
                formatedValue = string.Format(CultureInfo.InvariantCulture, header.Format, value);
            }

            if (header.Width < formatedValue.Length) { header.Width = formatedValue.Length; }

            header.IsEmpty = false;
            return formatedValue;
        }
    }
}