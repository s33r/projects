using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Automation
{
    /// <summary>
    /// In order to send a dependency to our script, we have to ensure that it has been loaded, not just referenced.
    /// </summary>
    internal static class ScriptDependencies
    {

        public static string EnsureDependencies()
        {
            Type t1 = typeof(Newtonsoft.Json.JsonArrayAttribute);

            return string.Join('\n', t1.ToString());
        }
    }
}
