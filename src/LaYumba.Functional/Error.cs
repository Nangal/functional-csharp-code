namespace LaYumba.Functional
{
   public static partial class F
   {
      public static Error Error(string message) => new Error(message);
   }

   public static class ErrorExt
   {
      public static Either<Error, R> ToEither<R>(this R value) => value;
      public static Either<Error, R> ToEither<R>(this Error err) => err;
   }

   public class Error
   {
      public virtual string Message { get; }
      public override string ToString() => Message;
      protected Error() { }
      internal Error(string Message) { this.Message = Message; }

      public static implicit operator Error(string m) => new Error(m);
   }
}
