using System.Collections.Generic;
using System.Linq;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Examples.Bind
{
   class SurveyOptionalAge
   {
      class Person
      {
         public Option<int> Age { get; set; }
      }

      static IEnumerable<Person> Population => new[]
      {
         new Person { Age = Some(33) },
         new Person { }, // this person did not disclose her age
         new Person { Age = Some(37) },
      };

      internal static void _main()
      {
         var optionalAges = Population.Map(p => p.Age);
         // => [Some(33), None, Some(37)]

         var statedAges = Population.Bind(p => p.Age);
         // => [33, 37]

         var averageAge = statedAges.Average();
         // => 35
      }
   }
}
