using System;
using System.Collections.Generic;
using System.Linq;

namespace LaYumba.Functional
{
   using static F;

   public static partial class F
   {
      public static Validation<T> Valid<T>(T value) => new Validation<T>(value);
   }

   public struct Validation<T>
   {
      internal IEnumerable<Error> Errors { get; }
      internal T Value { get; }

      public bool IsValid { get; }

      // the Return function for Validation
      public static Func<T, Validation<T>> Return = t => Valid(t);

      // create a Validation in the Invalid state
      public static Validation<T> Fail(IEnumerable<Error> errors)
         => new Validation<T>(errors);

      public static Validation<T> Fail(params Error[] errors)
         => new Validation<T>(errors.AsEnumerable());

      private Validation(IEnumerable<Error> errors)
      {
         IsValid = false;
         Errors = errors;
         Value = default(T);
      }

      internal Validation(T right)
      {
         IsValid = true;
         Value = right;
         Errors = Enumerable.Empty<Error>();
      }

      public static implicit operator Validation<T>(Error left) => Fail(left);
      public static implicit operator Validation<T>(T right) => Valid(right);

      public TR Match<TR>(Func<IEnumerable<Error>, TR> Invalid, Func<T, TR> Valid)
         => IsValid ? Valid(this.Value) : Invalid(this.Errors);

      public Unit Match(Action<IEnumerable<Error>> Invalid, Action<T> Valid)
         => Match(Invalid.ToFunc(), Valid.ToFunc());

      public IEnumerator<T> AsEnumerable()
      {
         if (IsValid) yield return Value;
      }

      public override string ToString()
         => IsValid
            ? $"Valid({Value})"
            : $"Invalid([{string.Join(", ", Errors)}])";

      public override bool Equals(object obj) => this.ToString() == obj.ToString(); // hack
   }

   public static class Validation
   {
      public static Validation<R> Apply<T, R>(this Validation<Func<T, R>> func, Validation<T> arg)
      {
         if (func.IsValid && arg.IsValid)
            return func.Value(arg.Value);

         var errors = func.IsValid
            ? arg.Errors
            : arg.IsValid
               ? func.Errors
               : func.Errors.Concat(arg.Errors); // accumulate errors

         return Validation<R>.Fail(errors);
      }

      public static Validation<Func<T2, R>> Apply<T1, T2, R>
         (this Validation<Func<T1, T2, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.Curry), arg);

      public static Validation<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Validation<Func<T1, T2, T3, R>> @this, Validation<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Validation<RR> Map<R, RR>(this Validation<R> @this
         , Func<R, RR> func)
         => @this.IsValid
            ? func(@this.Value)
            : Validation<RR>.Fail(@this.Errors);

      public static Validation<Func<T2, R>> Map<T1, T2, R>(this Validation<T1> @this
         , Func<T1, T2, R> func)
          => @this.Map(func.Curry());

      public static Validation<Unit> ForEach<R>(this Validation<R> @this, Action<R> act)
         => Map(@this, act.ToFunc());

      public static Validation<RR> Bind<R, RR>(this Validation<R> @this
         , Func<R, Validation<RR>> func)
          => @this.IsValid
            ? func(@this.Value)
            : Validation<RR>.Fail(@this.Errors);

      // LINQ

      public static Validation<R> Select<T, R>(this Validation<T> @this
         , Func<T, R> map) => @this.Map(map);

      public static Validation<RR> SelectMany<T, R, RR>(this Validation<T> @this
         , Func<T, Validation<R>> bind, Func<T, R, RR> project)
      {
         if (!@this.IsValid) return Validation<RR>.Fail(@this.Errors);
         var bound = bind(@this.Value);
         if (!bound.IsValid) return Validation<RR>.Fail(bound.Errors);
         return project(@this.Value, bound.Value);
      }
   }
}
