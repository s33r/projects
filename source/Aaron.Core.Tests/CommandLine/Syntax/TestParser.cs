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
using Aaron.Core.CommandLine.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Core.Tests.CommandLine.Syntax
{
    [TestClass]
    public class TestParser
    {
        [TestMethod]
        public void TestAliasParameter()
        {
            List<CommandLineError> errors = new List<CommandLineError>();

            Dictionary<string, Command> commands = new Dictionary<string, Command>
            {
                {
                    "command", new Command("command",
                        new List<Parameter>
                            {
                                new Parameter("test-param-1", "tp1"), new Parameter("test-param-2", "tp2"),
                            })
                },
            };


            string[] args = {"command", "--tp1"};
            List<IToken> tokens = Tokenizer.Parse(args, errors);

            ParsedCommandLine commandLine = Parser.Parse(tokens, commands, errors);

            Parameter parameter = commandLine.Command.Parameters
                                             .ToList()
                                             .Find(p => p.Name == "test-param-1");

            Assert.AreEqual("true", parameter.Value);
            Assert.AreEqual(false, commandLine.HasErrors);
        }

        [TestMethod]
        public void TestCommand()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            Dictionary<string, Command> commands = new Dictionary<string, Command>
            {
                {"command", new Command {Name = "command"}},
            };

            string[] args = {"command"};
            List<IToken> tokens = Tokenizer.Parse(args, errors);

            ParsedCommandLine commandLine = Parser.Parse(tokens, commands, errors);

            Assert.AreEqual("command", commandLine.Command.Name);
            Assert.AreEqual(0, commandLine.Parameters.Count);
            Assert.AreEqual(string.Empty, commandLine.Leftover);
            Assert.AreEqual(false, commandLine.HasErrors);
        }


        [TestMethod]
        public void TestCommandParams()
        {
            List<CommandLineError> errors = new List<CommandLineError>();

            Dictionary<string, Command> commands = new Dictionary<string, Command>
            {
                {
                    "command", new Command("command",
                        new List<Parameter>
                        {
                            new Parameter("test-param-1", false, "false"),
                            new Parameter("test-param-2", false, "false"),
                        })
                },
            };


            string[] args = {"command", "--test-param-1"};
            List<IToken> tokens = Tokenizer.Parse(args, errors);

            ParsedCommandLine commandLine = Parser.Parse(tokens, commands, errors);

            Assert.AreEqual("true", commandLine.Command.Parameters.ToDictionary()["test-param-1"].Value);
            Assert.AreEqual("false", commandLine.Command.Parameters.ToDictionary()["test-param-2"].Value);
            Assert.AreEqual(false, commandLine.HasErrors);
        }


        [TestMethod]
        public void TestEmptyArgsDefault()
        {
            List<CommandLineError> errors = new List<CommandLineError>();

            Dictionary<string, Parameter> parameters =
                new Dictionary<string, Parameter> {{"test", new Parameter("test")}};

            string[] args = Array.Empty<string>();
            List<IToken> tokens = Tokenizer.Parse(args, errors);

            ParsedCommandLine commandLine = Parser.Parse(tokens, parameters, errors);

            Assert.IsNull(commandLine.Command);
            Assert.AreEqual(0, commandLine.Parameters.Count);
            Assert.AreEqual(string.Empty, commandLine.Leftover);
            Assert.AreEqual(true, commandLine.HasErrors);
        }

        [TestMethod]
        public void TestNullArgDefault()
        {
            List<CommandLineError> errors = new List<CommandLineError>();

            Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>
            {
                {"test", new Parameter("test")},
            };

            ParsedCommandLine commandLine = Parser.Parse(null, parameters, errors);

            Assert.IsNull(commandLine.Command);
            Assert.AreEqual(0, commandLine.Parameters.Count);
            Assert.AreEqual(string.Empty, commandLine.Leftover);
            Assert.AreEqual(false, commandLine.HasErrors);
        }


        [TestMethod]
        public void TestRequiredCommandParams()
        {
            List<CommandLineError> errors = new List<CommandLineError>();

            Dictionary<string, Command> commands = new Dictionary<string, Command>
            {
                {
                    "command", new Command("command",
                        new List<Parameter>
                        {
                            new Parameter("test-param-1", true, "false"), new Parameter("test-param-2", true, "false"),
                        })
                },
            };


            string[] args = {"command", "--test-param-1"};
            List<IToken> tokens = Tokenizer.Parse(args, errors);

            ParsedCommandLine commandLine = Parser.Parse(tokens, commands, errors);

            Assert.AreEqual(true, commandLine.HasErrors);
        }

        [TestMethod]
        public void TestUnknownParameter()
        {
            List<CommandLineError> errors = new List<CommandLineError>();

            Dictionary<string, Command> commands = new Dictionary<string, Command>
            {
                {
                    "command", new Command("command",
                        new List<Parameter>
                            {
                                new Parameter("test-param-1", "tp1"), new Parameter("test-param-2", "tp2"),
                            })
                },
            };


            string[] args = {"command", "--unknown-param"};
            List<IToken> tokens = Tokenizer.Parse(args, errors);

            ParsedCommandLine commandLine = Parser.Parse(tokens, commands, errors);

            Assert.AreEqual(true, commandLine.HasErrors);
        }
    }
}