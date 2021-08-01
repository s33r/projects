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