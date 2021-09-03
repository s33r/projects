using System;

namespace Aaron.Automation
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //ScriptDependencies.EnsureDependencies();

#if DEBUG
            args = new[] { "./TestScript.csx" };
#endif
            Status status = new Status()
            {
                ProgramArguments = args,
            };

            Pipeline pipeline = new Pipeline
            {
                Load = Script.Load,
                Parse = Script.Parse,
                Compile = Script.Compile,
                Run = Script.Run
            };

            pipeline.Execute(status);
        }
    }
}
