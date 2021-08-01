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
        public void CollapsesFlagParam()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            string[] args = {"command", "--flag-param", "--param-name", "param-value"};

            List<IToken> result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual("true", result[1].Value);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void CollapsesFlagParamEnd()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            string[] args = {"command", "--flag-param"};

            List<IToken> result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(2, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual("true", result[1].Value);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void CollapsesParameters()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            string[] args = {"command", "--param-name", "param-value"};

            List<IToken> result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(2, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void CollapsesSkippedArgs()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            string[] args = {"command", "--", "--param-name", "param-value"};

            List<IToken> result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(2, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(BreakToken));
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void ConvertsFlags()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            string[] args = {"command", "/flag", "--param-name", "param-value"};

            List<IToken> result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType(result[1], typeof(ParameterToken));
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void NoArguments()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            string[] args = Array.Empty<string>();

            List<IToken> result = Tokenizer.Parse(args, errors);

            Assert.AreEqual(result.Count, 0);
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void NullArguments()
        {
            List<CommandLineError> errors = new List<CommandLineError>();
            List<IToken> result = Tokenizer.Parse(null, errors);

            Assert.AreEqual(result.Count, 0);
            Assert.AreEqual(1, errors.Count);
        }
    }
}