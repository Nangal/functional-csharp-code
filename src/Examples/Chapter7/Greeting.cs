namespace Examples.Chapter7
{
   public class Greeting
   {
      public string Value { get; }
      public Greeting(string value) { Value = value; }
      public static implicit operator Greeting(string s) => new Greeting(s);
      public override string ToString() => Value;
   }
}