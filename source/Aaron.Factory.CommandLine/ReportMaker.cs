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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Aaron.Core.Extensions;
using Aaron.Core.TextFiles;
using Aaron.Factory.CommandLine.Data;

namespace Aaron.Factory.CommandLine
{
    public enum ReportFormats
    {
        Markdown,
        Csv,
    }

    public static class ReportMaker
    {
        public static void CreateReport(string outputLocation, IEnumerable<Recipe> recipes,
                                        ReportFormats format = ReportFormats.Markdown)
        {
            using StreamWriter output = new StreamWriter(File.OpenWrite(outputLocation));
            output.BaseStream.SetLength(0);

            CreateReport(output, recipes, format);

            output.Flush();
        }


        public static void CreateReport(StreamWriter output, IEnumerable<Recipe> recipes,
                                        ReportFormats format = ReportFormats.Markdown)
        {
            if (output is null) { throw new ArgumentNullException(nameof(output)); }

            if (recipes is null) { throw new ArgumentNullException(nameof(recipes)); }

            switch (format)
            {
                case ReportFormats.Csv:
                    CreateCsvReport(output, recipes);
                    return;
                case ReportFormats.Markdown:
                default:
                    CreateMarkdownReport(output, recipes);
                    return;
            }
        }

        private static void CreateCsvReport(StreamWriter output, IEnumerable<Recipe> recipes)
        {
            throw new NotImplementedException();
        }

        private static void CreateMarkdownReport(StreamWriter output, IEnumerable<Recipe> recipes)
        {
            TableLayout layout = new TableLayout();

            layout
                .AddHeader("I", HorizontalAlignment.Center)
                .AddHeader("Time", HorizontalAlignment.Center)
                .AddHeader("Name")
                .AddHeader("Output 1 - Name")
                .AddHeader("Output 1 - Supply", HorizontalAlignment.Center)
                .AddHeader("Output 2 - Name")
                .AddHeader("Output 2 - Supply", HorizontalAlignment.Center)
                .AddHeader("Output 3 - Name")
                .AddHeader("Output 3 - Supply", HorizontalAlignment.Center)
                .AddHeader("Output 4 - Name")
                .AddHeader("Output 4 - Supply", HorizontalAlignment.Center)
                .AddHeader("Output 5 - Name")
                .AddHeader("Output 5 - Supply", HorizontalAlignment.Center)
                .AddHeader("Input 1 - Name")
                .AddHeader("input 1 - Demand", HorizontalAlignment.Center)
                .AddHeader("Input 2 - Name")
                .AddHeader("input 2 - Demand", HorizontalAlignment.Center)
                .AddHeader("Input 3 - Name")
                .AddHeader("input 3 - Demand", HorizontalAlignment.Center)
                .AddHeader("Input 4 - Name")
                .AddHeader("input 4 - Demand", HorizontalAlignment.Center)
                .AddHeader("Input 5 - Name")
                .AddHeader("input 5 - Demand", HorizontalAlignment.Center);


            foreach (Recipe recipe in recipes)
            {
                List<string> columns = new List<string>
                {
                    recipe.Instances.ToString(CultureInfo.InvariantCulture),
                    recipe.Time.ToString(CultureInfo.InvariantCulture),
                    recipe.Name,
                };

                foreach (OutputPort port in recipe.OutputPorts)
                {
                    if (port.Active)
                    {
                        columns.Add(port.Name);
                        columns.Add(port.Supply.Truncate(4).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        columns.Add(string.Empty);
                        columns.Add(string.Empty);
                    }
                }

                foreach (InputPort port in recipe.InputPorts)
                {
                    if (port.Active)
                    {
                        columns.Add(port.Name);
                        columns.Add(port.Demand.Truncate(4).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        columns.Add(string.Empty);
                        columns.Add(string.Empty);
                    }
                }

                layout.AddRow(columns);
            }

            IEnumerable<string> lines = layout.GetTable().Select(r => "| " + string.Join(" | ", r));


            output.WriteLine(lines.First());
            output.WriteLine(lines.First().Mask('-', '|'));

            foreach (string row in lines.Skip(1)) { output.WriteLine(row); }

            output.Flush();
        }
    }
}