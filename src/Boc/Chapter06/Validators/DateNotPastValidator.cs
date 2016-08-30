using Boc.Services;
using Boc.Commands;
using LaYumba.Functional;
using Boc.Domain;

namespace Boc.EitherImpl.Services
{
   public class DateNotPastValidator : IValidator<TransferOn>
   {
      private readonly IDateTimeService clock;

      public DateNotPastValidator(IDateTimeService clock)
      {
         this.clock = clock;
      }

      public int Priority { get; } = 1;

      public Either<Error, TransferOn> Validate(TransferOn request)
         => (request.Date.Date <= clock.UtcNow.Date)
            ? Errors.TransferDateIsPast
            : request.ToEither();
   }
}