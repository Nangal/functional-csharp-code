using LaYumba.Functional;

namespace Boc.ValidImpl.Services
{
   public interface IValidator<T>
   {
      Validation<T> Validate(T request);
   }
}