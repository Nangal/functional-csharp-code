using LaYumba.Functional;

namespace Boc.EitherImpl.Services
{
   public interface IValidator<T>
   {
      int Priority { get; }
      Either<Error, T> Validate(T request);
   }
}