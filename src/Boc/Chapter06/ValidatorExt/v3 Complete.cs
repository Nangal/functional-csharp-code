using LaYumba.Functional;
using System.Collections.Generic;
using System.Linq;

namespace Boc.EitherImpl.Services.Complete
{
   // Performs validation in such a way that all errors are aggregated into a list
   // The same can be more elegantly achieved using TraverseA
   public static class ValidatorExt
   {
      public static Either<List<Error>, T> Validate<T>
         (this IEnumerable<IValidator<T>> validators, T request)
      {
         var lefts = validators
            .Map(v => v.Validate(request))
            .Where(e => e.IsLeft);

         if (lefts.Count() == 0) return request;
         return lefts.Map(e => e.Match(left => left, null)).ToList();
      }
   }
}
