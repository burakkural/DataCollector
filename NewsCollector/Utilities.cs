using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCollector
{
   public static class Utilities
    {
        public static void WriteConsole(News obj)
        {
            Console.WriteLine(obj.Title);
            Console.WriteLine(obj.Description);
            Console.WriteLine(obj.Url);
            Console.WriteLine(obj.ImageUrl);
            Console.WriteLine(new string('*', 30));
        }

        public static String ToClearString(ref String str) => str.Replace(System.Environment.NewLine, string.Empty)
            .Replace(" ",string.Empty);
    }
}
