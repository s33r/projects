using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;

namespace Aaron.Automation
{
    public class Status
    {
        public string[] ProgramArguments { get; set; }
        public string FileLocation { get; set; }
        public string[] ScriptArguments { get; set; }
        public string RawText { get; set; }
        public List<string> LoadMessages { get; } = new List<string>();
        public List<string> RunMessages { get; } = new List<string>();
        public SyntaxTree SyntaxTree { get; set; }
        public EmitResult CompileResult { get; set; }
        public byte[] CompiledBytes { get; set; }
        public bool HasError { get; set; }

        public string LastAction { get; set; }

        public IEnumerable<string> GetMessages()
        {
            List<string> result = new List<string>();

            result.AddRange(LoadMessages);

            if (SyntaxTree != null)
            {
                result.AddRange(SyntaxTree.GetDiagnostics().Select(d => d.ToString()));
            }

            if (CompileResult != null)
            {
                result.AddRange(CompileResult.Diagnostics.Select(d => d.ToString()));
            }

            result.AddRange(RunMessages);

            return result;
        }


    }
}
