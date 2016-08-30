using System;
using System.Collections.Generic;
using System.Linq;

namespace LaYumba.Functional
{
   using static F;

   public struct Option<T> : IEquatable<NoneType>, IEquatable<Option<T>>
   {
      public static readonly Option<T> None = new Option<T>();

      internal T Value { get; }
      public bool IsSome { get; }
      public bool IsNone => !IsSome;

      internal Option(T value, bool isSome)
      {
         IsSome = isSome;
         Value = value;
      }

      public static implicit operator Option<T>(T value) => Some(value);
      public static implicit operator Option<T>(NoneType _) => None;
      
      public R Match<R>(Func<R> None, Func<T, R> Some)
          => IsSome ? Some(Value) : None();

      public IEnumerable<T> AsEnumerable()
      {
         if (IsSome) yield return Value;
      }

      public bool Equals(Option<T> other) 
         => this.IsSome == other.IsSome 
         && (this.IsNone || this.Value.Equals(other.Value));

      public bool Equals(NoneType other) => IsNone;

      public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
      public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

      public override string ToString() => IsSome ? $"Some({Value})" : "None";
   }

   public static class Option
   {
      public static Func<T, Option<T>> Return<T>() => t => Some(t);

      public static Option<T> Of<T>(T value)
         => new Option<T>(value, value != null);
      
      public static Option<R> Apply<T, R>
         (this Option<Func<T, R>> @this, Option<T> arg)
         => @this.IsSome && arg.IsSome
            ? Some(@this.Value(arg.Value))
            : None;

      public static Option<Func<T2, R>> Apply<T1, T2, R>
         (this Option<Func<T1, T2, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.Curry), arg);

      public static Option<Func<T2, T3, R>> Apply<T1, T2, T3, R>
         (this Option<Func<T1, T2, T3, R>> @this, Option<T1> arg)
         => Apply(@this.Map(F.CurryFirst), arg);

      public static Option<R> Bind<T, R>(this Option<T> @this, Func<T, Option<R>> func)
          => @this.IsSome
              ? func(@this.Value)
              : None;

      public static IEnumerable<R> Bind<T, R>(this Option<T> @this
         , Func<T, IEnumerable<R>> func)
          => @this.AsEnumerable().Bind(func);

      public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
         => Map(@this, action.ToFunc());

      public static T GetOrElse<T>(this Option<T> opt, T defaultValue) 
         => opt.Match(
            Some: value => value,
            None: () => defaultValue);

      public static T GetOrElse<T>(this Option<T> @this, Func<T> fallback) 
         => @this.Match(
            Some: value => value,
            None: fallback);

      public static Option<R> Map<T, R>(this Option<T> @this, Func<T, R> func)
          => @this.IsSome
              ? Some(func(@this.Value))
              : None;

      public static Option<Func<T2, R>> Map<T1, T2, R>
         (this Option<T1> @this, Func<T1, T2, R> func)
          => @this.Map(func.Curry());

      public static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>
         (this Option<T1> @this, Func<T1, T2, T3, R> func)
          => @this.Map(func.CurryFirst());

      public static IEnumerable<Option<R>> Traverse<T, R>(this Option<T> @this
         , Func<T, IEnumerable<R>> func)
         => @this.Match(
            () => List(Option<R>.None),
            t => func(t).Map(tt => Some(tt)));

      // LINQ

      public static Option<R> Select<T, R>(this Option<T> @this, Func<T, R> func)
         => @this.Map(func);

      public static Option<T> Where<T>(this Option<T> @this, Func<T, bool> predicate)
         => @this.IsSome && predicate(@this.Value) ? @this : None;

      public static Option<RR> SelectMany<T, R, RR>(this Option<T> @this
         , Func<T, Option<R>> bind, Func<T, R, RR> project)
      {
         var bound = @this.Bind(bind);
         return bound.IsSome
            ? Some(project(@this.Value, bound.Value))
            : None;
      }
   }
}
