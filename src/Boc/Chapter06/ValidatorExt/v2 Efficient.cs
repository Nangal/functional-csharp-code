using LaYumba.Functional;
using System.Collections.Generic;
using System.Linq;

namespace Boc.EitherImpl.Services.Efficient
{
   public static class ValidatorExt
   {
      public static Either<Error, T> Validate<T>(this IEnumerable<IValidator<T>> validators
         , T request)
         => validators
            .OrderBy(v => v.Priority)
            .Aggregate(Either.Of<Error, T>(request)
               , (either, validator) => either.Bind(validator.Validate));
   }
}
