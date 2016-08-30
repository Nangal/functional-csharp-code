using System;
using LaYumba.Functional;

namespace Examples.Chapter4
{
   using System.Linq;
   using static F;

   public class Examples
   {
      public static void List_Map()
      {
         Func<int, int> plus3 = x => x + 3;

         var a = new[] { 2, 4, 6 };
         // => [2, 4, 6]

         var b = a.Map(plus3);
         // => [5, 7, 9]
      }

      public static void List_ForEach()
      {
         Enumerable.Range(1, 5).ForEach(Console.Write);
      }

      internal static void _main()
      {
         int counter = 0;
         Func<string, string> ToUpper = s => s.ToUpper();
         Func<string, string> PrependCounter = s => string.Join(" ", ++counter, s);

         var name = Some("Enrico");

         name.Map(ToUpper)
             .Map(PrependCounter)
             .ForEach(Console.Out.WriteLine);

         var names = new[] { "Constance", "Brunhilde" };

         names.Map(ToUpper)
             .Map(PrependCounter)
             .ForEach(Console.Out.WriteLine);

         Console.Read();
      }
   }
}
