using System;
using Aaron.Core.CommandLine;
using Aaron.Core.CommandLine.Syntax;

namespace Aaron.Automation.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fakeArguments1 = {
                "hello",
                "--file",
                "open",
                "/ok",
                "super",
                "--",
                "this",
                "should",
                "be",
                "combined",
            };


            string[] fakeArguments2 = {
                "hello",
                "--",
                "this",
                "should",
                "be",
                "combined",
            };

            ArgumentParser parser = new ArgumentParser();

            parser.Commands
                .AddCommand(new Command()
                {
                    Name = "hello",
                    ShortDescription = "Say hello",
                    LongDescription = "Say hello to the world!",
                    OnExecute = (c) => Console.WriteLine("Hello World!"),
                })
                .AddCommand(new Command()
                {
                    Name = "test",
                    ShortDescription = "Do a test",
                    LongDescription = "Display the environment",
                    OnExecute = Console.WriteLine,
                });


            parser.Run(fakeArguments2);
        }


        
    }
}
