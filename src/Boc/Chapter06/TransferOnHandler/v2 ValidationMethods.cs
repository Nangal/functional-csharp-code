using Boc.Services;
using Boc.Commands;
using LaYumba.Functional;
using System.Text.RegularExpressions;
using Boc.Domain;

namespace Boc.EitherImpl.Services.ValidationMethods
{
   public class TransferOnHandler
   {
      IWriteRepository<TransferOn> repository;
      IDateTimeService clock;
      static Regex bicRegex = new Regex("[A-Z]{11}");

      public Either<Error, Unit> Handle(TransferOn request)
         => Either.Of<Error, TransferOn>(request)
            .Bind(ValidateBic)
            .Bind(ValidateDate)
            .Bind(repository.Save);

      public Either<Error, Unit> HandleLINQ(TransferOn request)
         => from r1 in ValidateBic(request)
            from r2 in ValidateDate(r1)
            from unit in repository.Save(r2)
            select unit;

      Either<Error, TransferOn> ValidateBic(TransferOn request)
      {
         if (!bicRegex.IsMatch(request.Bic)) return Errors.InvalidBic;
         else return request;
      }

      Either<Error, TransferOn> ValidateDate(TransferOn request)
      {
         if (request.Date.Date <= clock.UtcNow.Date) return Errors.TransferDateIsPast;
         else return request;
      }
   }
}
