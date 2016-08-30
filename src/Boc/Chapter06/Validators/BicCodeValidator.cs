using Boc.Services;
using Boc.Commands;
using LaYumba.Functional;
using System.Text.RegularExpressions;
using Boc.Domain;

namespace Boc.EitherImpl.Services
{
   public class BicCodeValidator : IValidator<Transfer>
   {
      private readonly Regex regex = new Regex("[A-Z]{11}");
      public int Priority { get; } = 1;

      public Either<Error, Transfer> Validate(Transfer request)
      {
         if (regex.IsMatch(request.Bic.ToUpper()))
            return Errors.InvalidBic;
         return request;
      }
   }
}