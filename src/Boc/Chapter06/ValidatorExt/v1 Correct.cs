using LaYumba.Functional;
using System.Collections.Generic;
using System.Linq;

namespace Boc.EitherImpl.Services.Correct
{
   public static class ValidatorExt
   {
      public static Either<Error, T> Validate<T>
         (this IEnumerable<IValidator<T>> validators
         , T request)
         => validators
            .Aggregate(request.ToEither()
               , (either, validator) => either.Bind(validator.Validate));
   }
}
