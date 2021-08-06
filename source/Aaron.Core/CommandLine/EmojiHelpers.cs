using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.Core.CommandLine
{
    public static partial class Emoji
    {

        public static IEnumerable<string> ListEmoji()
        {
            Type type = typeof(Emoji);


            IEnumerable<string> result = type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.PropertyType == typeof(string))
                .Select(info => info.GetValue(null)?.ToString());

            return result;
        }
    }
}
