using LaYumba.Functional;
using System.Collections.Generic;

namespace Boc.ValidImpl.Services
{
   public static class ValidatorExt
   {
      public static Validation<T> Validate<T>
         (this IEnumerable<IValidator<T>> validators, T request)
         => validators
            .Traverse(v => v.Validate(request))
            .Map(_ => request);
   }
}
