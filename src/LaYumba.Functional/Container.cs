using System;

namespace LaYumba.Functional
{
   public struct Container<T>
   {
      internal T Value { get; }
      internal Container(T value) { Value = value; }
      public override string ToString() => $"Container({Value})";

      public static implicit operator Container<T>(T value) => Container.Of(value);
      public static implicit operator T(Container<T> c) => c.Value;
   }

   public static class Container
   {
      public static Container<T> Of<T>(T value)
         => new Container<T>(value);

      public static Container<R> Map<T, R>(this Container<T> @this
         , Func<T, R> func) 
         => func(@this);
         // alternatively, without implicit lifting/lowering
         // => Container.Of(func(@this.Value));

      public static Container<R> Bind<T, R>(this Container<T> @this
         , Func<T, Container<R>> func)
         => func(@this);

      public static Container<Unit> ForEach<T>(this Container<T> @this
         , Action<T> action)
         => action.ToFunc()(@this);
   }
}
