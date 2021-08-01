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
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Core.Tests.CommandLine
{
    [TestClass]
    public class TestArgumentParser
    {
        [TestMethod]
        public void TestActionInvokedArguments()
        {
            string[] args = null;
            bool functionCalled = false;

            ArgumentParser parser = new ArgumentParser();
            parser.DefaultAction += line => functionCalled = true;

            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, functionCalled);
        }


        [TestMethod]
        public void TestAddCommand()
        {
            string[] args = {"command"};
            bool functionCalled = false;

            Command command = new Command("command");
            command.OnExecute += line => functionCalled = true;

            ArgumentParser parser = new ArgumentParser();
            parser.Commands.AddCommand(command);


            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, functionCalled);
        }

        [TestMethod]
        public void TestAddDefaultParameter()
        {
            string[] args = {"--test", "f"};
            bool functionCalled = false;

            Parameter parameter = new Parameter("test");


            ArgumentParser parser = new ArgumentParser();
            parser.DefaultBuilder.AddParameter(parameter);

            parser.DefaultAction += line =>
            {
                functionCalled = true;
                Assert.AreEqual("f", line.Parameters[0].Value);
            };

            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, functionCalled);
        }

        [TestMethod]
        public void TestEmptyArguments()
        {
            ArgumentParser parser = new ArgumentParser();

            ParsedCommandLine commandLine = parser.Run(Array.Empty<string>());

            Assert.AreEqual(true, commandLine.HasErrors);
        }


        [TestMethod]
        public void TestErrorActionInvokedArguments()
        {
            string[] args = null;
            bool functionCalled = false;

            ArgumentParser parser = new ArgumentParser();
            parser.ErrorAction += line => functionCalled = true;

            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, functionCalled);
        }

        [TestMethod]
        public void TestFatalError()
        {
            string[] args = {"--oops", "f"};
            bool functionCalled = false;

            Parameter parameter = new Parameter("test") {Required = true};


            ArgumentParser parser = new ArgumentParser();
            parser.DefaultBuilder.AddParameter(parameter);

            parser.FatalErrorAction += line => { functionCalled = true; };

            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, functionCalled);
        }

        [TestMethod]
        public void TestNullArguments()
        {
            string[] args = null;
            ArgumentParser parser = new ArgumentParser();

            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, commandLine.HasErrors);
        }
    }
}