using System;
using System.Collections.Generic;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Core.Tests.CommandLine.Tokens
{
    [TestClass]
    public class TestTokenizer
    {
        [TestMethod]
        public void NoArguments()
        {
            List<CommandLineError> errors = new();
            string[] args = Array.Empty<string>();

            var result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(result.Count, 0);
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void NullArguments()
        {
            List<CommandLineError> errors = new();
            var result = Tokenizer.Parse(null, errors);

            Assert.AreEqual(result.Count, 0);
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void CollapsesParameters()
        {
            List<CommandLineError> errors = new();
            string[] args =
            {
                "command",
                "--param-name",
                "param-value",
            };

            var result = Tokenizer.Parse(args, errors);

            Assert.AreEqual( 2, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void ConvertsFlags()
        {
            List<CommandLineError> errors = new();
            string[] args =
            {
                "command",
                "/flag",
                "--param-name",
                "param-value",
            };

            var result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void CollapsesFlagParam()
        {
            List<CommandLineError> errors = new();
            string[] args =
            {
                "command",
                "--flag-param",
                "--param-name",
                "param-value",
            };

            var result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual("true", result[1].Value);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void CollapsesFlagParamEnd()
        {
            List<CommandLineError> errors = new();
            string[] args =
            {
                "command",
                "--flag-param",
            };

            var result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(2, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual("true", result[1].Value);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void CollapsesSkippedArgs()
        {
            List<CommandLineError> errors = new();
            string[] args =
            {
                "command",
                "--",
                "--param-name",
                "param-value",
            };

            var result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(2, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(BreakToken));
            Assert.AreEqual(0, errors.Count);
        }
    }
}
