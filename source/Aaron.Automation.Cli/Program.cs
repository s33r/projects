using System;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Automation.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ArgumentParser parser = new ArgumentParser();

            parser.Commands
                  .AddCommand(new Command
                  {
                      Name = "hello",
                      ShortDescription = "Say hello",
                      LongDescription = "Say hello to the world!",
                      OnExecute = c => Console.WriteLine("Hello World!"),
                  })
                  .AddCommand(new Command
                  {
                      Name = "test",
                      ShortDescription = "Do a test",
                      LongDescription = "Display the environment",
                      OnExecute = Console.WriteLine,
                  });


            ParsedCommandLine commandLine = parser.Run(args);
        }
    }
}