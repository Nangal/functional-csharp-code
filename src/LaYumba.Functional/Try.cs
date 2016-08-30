using System;
using static LaYumba.Functional.F;

namespace LaYumba.Functional
{
   public delegate Exceptional<T> Try<T>();
   
   public static class TryExt
   {
      public static R Match<T, R>(this Try<T> @this, Func<Exception, R> Exception, Func<T, R> Success)
      {
         var result = @this.Try();
         return result.Exception
             ? Exception(result.Ex)
             : Success(result.Value);
      }

      public static Unit Match<T>(this Try<T> @this, Action<Exception> Exception, Action<T> Success)
      {
         var result = @this.Try();

         if (result.Exception) Exception(result.Ex);
         else Success(result.Value);

         return Unit();
      }

      public static Option<T> ToOption<T>(this Try<T> @this)
      {
         var res = @this.Try();
         return res.Exception
             ? None
             : Some(res.Value);
      }

      public static Exceptional<T> Try<T>(this Try<T> @this)
      {
         try { return @this(); }
         catch (Exception e) { return Exceptional.Of<T>(e); }
      }
      
      public static Try<R> Map<T, R>(this Try<T> @this, Func<T, R> mapper) => () =>
      {
         var result = @this.Try();
         return result.Exception
             ? Exceptional.Of<R>(result.Ex)
             : mapper(result.Value);
      };

      public static Try<Func<T2, R>> Map<T1, T2, R>(this Try<T1> @this, Func<T1, T2, R> func) =>
          @this.Map(func.Curry());

      public static Try<Func<T2, Func<T3, R>>> Map<T1, T2, T3, R>(this Try<T1> @this, Func<T1, T2, T3, R> func) =>
          @this.Map(func.Curry());

      public static Try<R> Bind<T, R>(this Try<T> @this, Func<T, Try<R>> binder) => () =>
      {
         var res = @this.Try();
         return res.Exception
             ? Exceptional.Of<R>(res.Ex)
             : binder(res.Value).Try();
      };

      //public static Try<R> Bind<T, R>(this Try<T> @this, Func<T, Try<R>> Succ, Func<Exception, Try<R>> Fail) => () =>
      //{
      //   var res = @this.Try();
      //   return res.Exception
      //       ? Fail(res.Ex).Try()
      //       : Succ(res.Value).Try();
      //};
   }
}
