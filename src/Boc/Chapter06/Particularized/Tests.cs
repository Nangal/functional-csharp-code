using LaYumba.Functional;
using System.Linq;
using NUnit.Framework;

namespace Boc.ValidImpl.Services.Test
{
   using static F;
   using static Assert;

   class InvalidError : Error { }

   class AlwaysValid<T> : IValidator<T>
   {
      public Validation<T> Validate(T t) => t;
   }

   class AlwaysInvalid<T> : IValidator<T>
   {
      public Validation<T> Validate(T t) => new InvalidError();
   }

   
   public class CommandExt_Test
   {
      IValidator<int> Valid => new AlwaysValid<int>();
      IValidator<int> Invalid => new AlwaysInvalid<int>();

      [Test]
      public void Succeed() => AreEqual(
         actual: List(Valid, Valid).Validate(1),
         expected: Valid(1)
      );

      [Test]
      public void WhenNoValidators_ThenSucceed() => AreEqual(
         actual: List<IValidator<int>>().Validate(1),
         expected: Valid(1)
      );

      [Test]
      public void Fail() => AreEqual(
         actual: List(Valid, Invalid)
            .Validate(1)
            .Match(
               Valid: (i) => -100,
               Invalid: (errs) => errs.Count()),
         expected: 1
      );

      [Test]
      public void FailWithSeveralErrors() => AreEqual(
         actual: List(Valid, Invalid, Invalid, Valid).Validate(1).Match(
            Valid: (i) => -100,
            Invalid: (errs) => errs.Count()),
         expected: 2
      );
   }
}