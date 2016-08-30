namespace Examples.Chapter7
{
   public class Name
   {
      public string Value { get; }
      public Name(string value) { Value = value; }
      public static implicit operator Name(string s) => new Name(s);
      public override string ToString() => Value;
   }
}