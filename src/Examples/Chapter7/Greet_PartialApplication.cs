using System;
using LaYumba.Functional;

namespace Examples.Chapter7
{
   using static Console;

   public static class Greet_PartialApplication
   {
      internal static void _main()
      {
         Func<Greeting, Name, Greeting> greet = (gr, name) => $"{gr}, {name}";
         Func<Greeting, Func<Name, Greeting>> greetWith = gr => name => $"{gr}, {name}";

         var names = new Name[] { "Tristan", "Ivan" };

         names.Map(g => greet("Hello", g)).ForEach(WriteLine);
         // prints: Hello, Tristan
         //         Hello, Ivan

         var greetFormally = greetWith("Good evening");
         names.Map(greetFormally).ForEach(WriteLine);
         // prints: Good evening, Tristan
         //         Good evening, Ivan

         var greetInformally = greet.Apply("Hey");
         names.Map(greetInformally).ForEach(WriteLine);
         // prints: Hey, Tristan
         //         Hey, Ivan

         var greetNostalgically = greet.Curry()("Arrivederci");
         names.Map(greetNostalgically).ForEach(WriteLine);
         // prints: Arrivederci, Tristan
         //         Arrivederci, Ivan
      }
   }
}