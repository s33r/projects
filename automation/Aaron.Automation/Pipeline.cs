using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Automation
{
    public delegate void PipelineAction(Status status);

    public class Pipeline : IEnumerable<PipelineAction>
    {
        public PipelineAction Load { get; set; }
        public PipelineAction Parse { get; set; }
        public PipelineAction Compile { get; set; }
        public PipelineAction Run { get; set; }

        public void Execute(Status status)
        {
            foreach (PipelineAction action in this)
            {
                action(status);
                status.LastAction = nameof(action);
            }

            if (status.HasError)
            {
                foreach (string message in status.GetMessages())
                {
                    Console.WriteLine(message);
                }
            }
        }

        public IEnumerator<PipelineAction> GetEnumerator()
        {
            yield return Load;
            yield return Parse;
            yield return Compile;
            yield return Run;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
