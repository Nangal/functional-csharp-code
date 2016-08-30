using Boc.Services;
using Boc.Commands;
using LaYumba.Functional;
using System.Text.RegularExpressions;
using Boc.Domain;

namespace Boc.EitherImpl.Services
{
   public class IbanValidator : IValidator<Transfer>
   {
      private const string ERROR_MESSAGE = "The beneficiary's IBAN code is invalid";
      private readonly Regex regex = new Regex(
          "[A-Z]{2}[0-9]{2}[A-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}");

      public int Priority { get; } = 1;

      public Either<Error, Transfer> Validate(Transfer request)
      {
         if (regex.IsMatch(request.Iban))
            return Errors.InvalidBic;
         return request;
      }
   }
}