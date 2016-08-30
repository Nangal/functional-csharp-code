using System;
using System.Collections.Generic;
using System.Linq;

namespace LaYumba.Functional
{
   using static F;

   public static class IEnumerableTraversable
   {
      static Func<IEnumerable<T>, T, IEnumerable<T>> Append<T>()
         => (ts, t) => ts.Append(t);

      // Exceptional

      public static Exceptional<IEnumerable<R>> Traverse<T, R>(this IEnumerable<T> list
         , Func<T, Exceptional<R>> func)
         => list.Aggregate(
            seed: Exceptional.Of(Enumerable.Empty<R>()),
            // Exceptional<[R]> -> T -> Exceptional<[R]>
            func: (optRs, t) => from rs in optRs
                                from r in func(t)
                                select rs.Append(r));

      // Option

      // applicative traverse, recursive (reference only)
      static Option<IEnumerable<R>> TraverseARec<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> func)
         => list.Match(
            Empty: () => Some(Enumerable.Empty<R>()),
            Otherwise: (t, ts) => Some(Cons<R>())
               .Apply(func(t))
               .Apply(ts.TraverseARec(func)));

      // applicative traverse
      public static Option<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> f)
         => list.Aggregate(
            seed: Some(Enumerable.Empty<R>()),
            func: (optRs, t) => Some(Append<R>())
                                   .Apply(optRs)
                                   .Apply(f(t)));

      // monadic traverse, recursive (reference only)
      static Option<IEnumerable<R>> TraverseMRec<T, R>(this IEnumerable<T> list
         , Func<T, Option<R>> func)
         => list.Match(
            Empty: () => Some(Enumerable.Empty<R>()),
            Otherwise: (t, ts) => from r in func(t)
                                  from rs in TraverseMRec(ts, func)
                                  select List(r).Concat(rs));

      // monadic traverse
      public static Option<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func) => Traverse(list, func);

      public static Option<IEnumerable<R>> Traverse<T, R>
         (this IEnumerable<T> list, Func<T, Option<R>> func)
         => list.Aggregate(
            seed: Some(Enumerable.Empty<R>()),
            // Option<[R]> -> T -> Option<[R]>
            func: (optRs, t) => from rs in optRs
                                from r in func(t)
                                select rs.Append(r));


      // Validation

      // applicative traverse, recursive (for reference only)
      public static Validation<IEnumerable<R>> TraverseARec<T, R>(this IEnumerable<T> list
         , Func<T, Validation<R>> func)
         => list.Match(
            Empty: () => Valid(Enumerable.Empty<R>()),
            Otherwise: (t, ts) => Valid(Cons<R>())
               .Apply(func(t))
               .Apply(ts.TraverseARec(func)));

      // applicative traverse
      public static Validation<IEnumerable<R>> Traverse<T, R>
         (this IEnumerable<T> @this, Func<T, Validation<R>> f)
         => @this.Aggregate(
            seed: Valid(Enumerable.Empty<R>()),
            func: (valRs, t) => Valid(Append<R>())
                                   .Apply(valRs)
                                   .Apply(f(t)));
   }
}