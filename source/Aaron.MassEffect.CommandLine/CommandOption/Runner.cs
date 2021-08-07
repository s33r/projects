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
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;
using Aaron.MassEffect.Coalesced;
using Aaron.MassEffect.Coalesced.Records;
using Aaron.MassEffect.Core;

namespace Aaron.MassEffect.CommandLine.CommandOption
{
    public enum DisplayMode
    {
        Fancy,
        Json,
        Csv,
    }

    public enum ActionMode
    {
        Get,
        Set,
    }

    public static class Runner
    {
        public static void Execute(ParsedCommandLine commandLine)
        {
            if (commandLine is null) { throw new ArgumentNullException(nameof(commandLine)); }

            Dictionary<string, Parameter> parameters = commandLine.Command.Parameters.ToDictionary();

            SearchCriteria criteria;
            DisplayMode displayMode = ParseDisplayMode(parameters["display"].Value);
            ActionMode actionMode = parameters["set"].Value == "true"
                ? ActionMode.Set
                : ActionMode.Get;

            if (parameters["path"].Value != null) { criteria = new SearchCriteria(parameters["path"].Value); }
            else
            {
                criteria = new SearchCriteria
                {
                    FileName = parameters["file"].Value,
                    SectionName = parameters["section"].Value,
                    EntryName = parameters["entry"].Value,
                    Index = parameters["index"].Value,
                };
            }

            criteria.Game = ParseGame(parameters["game"].Value);
            criteria.Mode = ParseSearchMode(parameters["type"].Value);


            if (actionMode == ActionMode.Get) { Search(criteria, displayMode); }
        }


        private static DisplayMode ParseDisplayMode(string mode)
        {
            if (string.IsNullOrEmpty(mode)) { return DisplayMode.Fancy; }

            string selectMode = mode.Trim().ToUpperInvariant();

            return selectMode switch
            {
                "FANCY" => DisplayMode.Fancy,
                "JSON" => DisplayMode.Json,
                "CSV" => DisplayMode.Csv,
                _ => DisplayMode.Fancy,
            };
        }

        private static Games ParseGame(string mode)
        {
            if (string.IsNullOrEmpty(mode)) { return Games.Me1; }

            string selectMode = mode.Trim().ToUpperInvariant();

            return selectMode switch
            {
                "ME1" => Games.Me1,
                "ME2" => Games.Me2,
                "ME3" => Games.Me3,
                _ => Games.Me1,
            };
        }

        private static SearchMode ParseSearchMode(string mode)
        {
            if (string.IsNullOrEmpty(mode)) { return SearchMode.Literal; }

            string selectMode = mode.Trim().ToUpperInvariant();

            return selectMode switch
            {
                "LITERAL" => SearchMode.Literal,
                "REGEX" => SearchMode.Regex,
                "SIMPLE" => SearchMode.Simple,
                _ => SearchMode.Literal,
            };
        }


        private static void Search(SearchCriteria criteria, DisplayMode displayMode)
        {
            Console.WriteLine($"{Emoji.Search} {criteria}");

            Container container = CoalescedFile.Load(criteria.Game,
                MassEffectConfiguration.Instance.Game[criteria.Game].CoalescedConfigurationLocation);


            foreach (IRecord record in Finder.FindRecords(criteria, container))
            {
                switch (displayMode)
                {
                    case DisplayMode.Csv:
                    case DisplayMode.Fancy:
                    case DisplayMode.Json:
                    default:
                        Console.WriteLine(record);
                        break;
                }
            }
        }
    }
}