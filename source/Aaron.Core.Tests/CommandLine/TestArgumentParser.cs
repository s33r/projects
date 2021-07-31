using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Core.Tests.CommandLine
{
    [TestClass]
    public class TestArgumentParser
    {

        [TestMethod]
        public void TestNullArguments()
        {
            ArgumentParser parser = new();

            ParsedCommandLine commandLine = parser.Run(null);

            Assert.AreEqual(true, commandLine.HasErrors);
        }

        [TestMethod]
        public void TestEmptyArguments()
        {
            ArgumentParser parser = new();

            ParsedCommandLine commandLine = parser.Run(Array.Empty<string>());

            Assert.AreEqual(true, commandLine.HasErrors);
        }

        [TestMethod]
        public void TestActionInvokedArguments()
        {
            bool functionCalled = false;

            ArgumentParser parser = new();
            parser.DefaultAction += line => functionCalled = true;

            ParsedCommandLine commandLine = parser.Run(null);

            Assert.AreEqual(true, functionCalled);
        }


        [TestMethod]
        public void TestErrorActionInvokedArguments()
        {
            bool functionCalled = false;

            ArgumentParser parser = new();
            parser.ErrorAction += line => functionCalled = true;

            ParsedCommandLine commandLine = parser.Run(null);

            Assert.AreEqual(true, functionCalled);
        }


        [TestMethod]
        public void TestAddCommand()
        {
            string[] args = {"command"};
            bool functionCalled = false;

            Command command = new ("command");
            command.OnExecute += line => functionCalled = true;

            ArgumentParser parser = new();
            parser.Commands.AddCommand(command);



            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, functionCalled);
        }

        [TestMethod]
        public void TestAddDefaultParameter()
        {
            string[] args = { "--test", "f" };
            bool functionCalled = false;

            Parameter parameter = new("test");
            

            ArgumentParser parser = new();
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
        public void TestFatalError()
        {
            string[] args = { "--oops", "f" };
            bool functionCalled = false;

            Parameter parameter = new("test") {Required = true};


            ArgumentParser parser = new();
            parser.DefaultBuilder.AddParameter(parameter);

            parser.FatalErrorAction += line =>
            {
                functionCalled = true;
            };

            ParsedCommandLine commandLine = parser.Run(args);

            Assert.AreEqual(true, functionCalled);
        }
    }
}
