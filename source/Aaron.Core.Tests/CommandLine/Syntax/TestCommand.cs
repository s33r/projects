using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Core.Tests.CommandLine.Syntax
{
    [TestClass]
    public class TestCommand
    {
        [TestMethod]
        public void TestCopyConstructor()
        {
            string longDescription = "longDescription";
            string shortDescription = "shortDescription";
            string name = "name";
            CommandAction action = e => Assert.AreEqual(name, e.Command.Name);


            Command originalCommand = new Command
            {
                LongDescription = longDescription, ShortDescription = shortDescription, Name = name, OnExecute = action,
            };


            Command newCommand = new Command(originalCommand);

            Assert.AreEqual(longDescription, newCommand.LongDescription);
            Assert.AreEqual(shortDescription, newCommand.ShortDescription);
            Assert.AreEqual(name, newCommand.Name);
            Assert.AreEqual(action, newCommand.OnExecute);
        }

        [TestMethod]
        public void TestCopyConstructorParameters()
        {
            string longDescription = "longDescription";
            string shortDescription = "shortDescription";
            string name = "name";
            CommandAction action = e => Assert.AreEqual(name, e.Command.Name);

            Parameter parameter = new Parameter("parameter", "p");


            Command originalCommand = new Command
            {
                LongDescription = longDescription, ShortDescription = shortDescription, Name = name, OnExecute = action,
            };

            originalCommand.Parameters.AddParameter(parameter);

            Command newCommand = new Command(originalCommand);

            Assert.AreEqual("parameter", newCommand.Parameters.ToList()[0].Name);
        }
    }
}