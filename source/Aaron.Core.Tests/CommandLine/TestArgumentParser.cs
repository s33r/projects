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