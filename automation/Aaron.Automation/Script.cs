using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


using Newtonsoft;

namespace Aaron.Automation
{
    internal static class Script
    {
        public static void Load(Status status)
        {
            if (status.ProgramArguments is null || status.ProgramArguments.Length <= 0)
            {
                status.HasError = true;
                status.LoadMessages.Add("No script file was provided.");
                return;
            }

            status.FileLocation = Path.GetFullPath(status.ProgramArguments[0]);
            status.ScriptArguments = status.ProgramArguments.Skip(1).ToArray();

            if (!File.Exists(status.FileLocation))
            {
                status.LoadMessages.Add($"The script file {status.FileLocation} could not be found.");
                status.HasError = true;
                return;
            }
            else
            {
                try
                {
                    status.RawText = File.ReadAllText(status.FileLocation);
                }
                catch (Exception exception)
                {
                    status.LoadMessages.Add($"The script file {status.FileLocation} could not be read. {exception.InnerException.Message}");
                    status.HasError = true;
                    return;
                }
            }
        }

        public static void Parse(Status status)
        {
            if (status.HasError)
            {
                return;
            }

            CSharpParseOptions parseOptions = new CSharpParseOptions(
                LanguageVersion.Latest,
                DocumentationMode.None,
                SourceCodeKind.Script
            );

            status.SyntaxTree = CSharpSyntaxTree.ParseText(status.RawText, parseOptions);
        }

        public static void Compile(Status status)
        {
            if (status.HasError)
            {
                return;
            }

            IEnumerable<SyntaxTree> syntaxTrees = new List<SyntaxTree>() { status.SyntaxTree };
            IEnumerable<MetadataReference> references = GetReferences();
            ImmutableArray<string> resolverPaths = ImmutableArray.Create(new string[]
            {
                Environment.CurrentDirectory,
            });


            CSharpCompilationOptions compileOptions = new CSharpCompilationOptions(
                OutputKind.ConsoleApplication,
                false, // reportSuppressedDiagnostics
                null, // mainTypeName
                null, //
                null,
                null,
                OptimizationLevel.Debug,
                false,
                true,
                null,
                null,
                default(ImmutableArray<byte>),
                false,
                Platform.AnyCpu,
                ReportDiagnostic.Default,
                4,
                null,
                true,
                false,
                null,
                new SourceFileResolver(resolverPaths, null),
                null,
                null,
                null,
                false,
                MetadataImportOptions.Public,
                NullableContextOptions.Disable
            );

            Compilation compilation = CSharpCompilation.Create("ScriptAssembly", syntaxTrees, references, compileOptions);

            using MemoryStream irStream = new MemoryStream();
            status.CompileResult = compilation.Emit(irStream);

            if (!status.CompileResult.Success)
            {
                status.HasError = true;
                return;
            }

            status.CompiledBytes = irStream.ToArray();
        }

        public static void Run(Status status)
        {
            if (status.HasError)
            {
                return;
            }

            Assembly assembly = Assembly.Load(status.CompiledBytes);
            MethodInfo entry = assembly.EntryPoint;

            entry.Invoke(null, null);
        }

        public static IEnumerable<MetadataReference> GetReferences()
        {
            Queue<AssemblyName> names = new Queue<AssemblyName>();

            foreach (AssemblyName name in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            {
                names.Enqueue(name);
            }

            while (names.Count > 0)
            {
                Assembly assembly = Assembly.Load(names.Dequeue());

                foreach (AssemblyName name in assembly.GetReferencedAssemblies())
                {
                    names.Enqueue(name);
                }
            }

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToList();
        }
    }
}
