using System;
using System.Threading.Tasks;

namespace LaYumba.Functional
{
   using static F;

   public static class ValidationTraversable
   {
      // Exceptional
      public static Exceptional<Validation<R>> TraverseA<T, R>
         (this Validation<T> val, Func<T, Exceptional<R>> func)
         => val.Match(
            Invalid: reasons => Exceptional.Of(Validation<R>.Fail(reasons)),
            Valid: t => Exceptional.Of(Validation<R>.Return).Apply(func(t)));

      public static Exceptional<Validation<R>> Traverse<T, R>
         (this Validation<T> tr, Func<T, Exceptional<R>> f)
         => tr.Match(
            Invalid: reasons => Exceptional.Of(Validation<R>.Fail(reasons)),
            Valid: t => from r in f(t) select Valid(r));

      // Task
      public static Task<Validation<R>> Traverse<T, R>(this Validation<T> @this
         , Func<T, Task<R>> func)
         => @this.Match(
               Invalid: reasons => Task.FromResult(Validation<R>.Fail(reasons)),
               Valid: t => Task.FromResult(Validation<R>.Return).Apply(func(t))
            );
   }

   public static class TaskTraversable
   {
      public static Validation<Task<R>> Traverse<T, R>(this Task<T> @this
         , Func<T, Validation<R>> func)
      { throw new NotImplementedException(); }
   }

   public static class ExceptionalTraversable
   {
      public static Validation<Exceptional<R>> Traverse<T, R>
         (this Exceptional<T> tr, Func<T, Validation<R>> f)
         => tr.Match(
            Exception: e => Valid(Exceptional.Of<R>(e)),
            Success: t => from r in f(t) select Exceptional.Of(r));
   }
}
