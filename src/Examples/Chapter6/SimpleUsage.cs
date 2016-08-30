using LaYumba.Functional;
using NUnit.Framework;
using static System.Math;

partial class Either_Example
{
   Either<string, double> Calc(double x, double y)
   {
      if (y == 0)
         return "y cannot be 0";

      if (x != 0 && Sign(x) != Sign(y))
         return "x / y cannot be negative";

      return Sqrt(x / y);
   }

   void UseMatch(double x, double y)
   {
      var message = Calc(x, y).Match(
         Right: z => $"Result: {z}",
         Left: err => $"Invalid input: {err}");
   }
}


partial class Either_Example
{
   [TestCase(1d, 0d, ExpectedResult = false, TestName = "When y is 0 Calc fails")]
   [TestCase(-90d, 10d, ExpectedResult = false, TestName = "When x/y < 0 Calc fails")]
   [TestCase(90d, 10d, ExpectedResult = true, TestName = "Otherwise Calc succeeds")]
   public bool TestCalc(double x, double y) => Calc(x, y).IsRight;
}
