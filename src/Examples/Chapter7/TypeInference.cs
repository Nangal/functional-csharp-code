using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace Examples.Chapter7
{
   public class TypeInference_Delegate
   {
      // 1. field
      Func<Greeting, Name, Greeting> GreeterField 
         = (gr, name) => $"{gr}, {name}";

      // 2. property
      Func<Greeting, Name, Greeting> GreeterProperty 
         => (gr, name) => $"{gr}, {name}";

      // 3. factory
      Func<Greeting, Name, Greeting> GreeterFactory() 
         => (gr, name) => $"{gr}, {name}";

      Func<Name, Greeting> CreateGreetingWith(Greeting greeting)
      {
         // 1. field
         return GreeterField.Apply(greeting);

         // 2. property
         return GreeterProperty.Apply(greeting);

         // 3. factory
         return GreeterFactory().Apply(greeting);
      }

      string separator = ", ";

      // does not compile
      //Func<Greeting, Name, Greeting> PoorGreeterField
      //   = (gr, name) => $"{gr}{separator}{name}";

      // 2. property
      Func<Greeting, Name, Greeting> GreeterProperty_
         => (gr, name) => $"{gr}{separator}{name}";

      Func<Greeting, Option<T>, Greeting> GreeterMethodGen<T>()
         => (gr, opt) => opt.Match(
            () => gr,
            (t) => $"{gr}{separator}{t}");
   }

   public class TypeInference_Method
   {
      // 1. method
      Greeting GreeterMethod(Greeting gr, Name name)
         => $"{gr}, {name}";

      // the line below does NOT compile!
      //Func<Name, Greeting> __GreetWith(Greeting greeting)
      //   => GreeterMethod.Apply(greeting);

      // the lines below compiles, but oh my!
      Func<Name, Greeting> GreetWith_1(Greeting greeting)
         => F.Apply<Greeting, Name, Greeting>(GreeterMethod, greeting);

      Func<Name, Greeting> _GreetWith_2(Greeting greeting)
         => new Func<Greeting, Name, Greeting>(GreeterMethod)
            .Apply(greeting);
   }
}

