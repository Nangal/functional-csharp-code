using Boc.Commands;
using Boc.EitherImpl.Services.Correct;
using LaYumba.Functional;
using System.Collections.Generic;

namespace Boc.EitherImpl.Services.InjectedValidators
{
   public class TransferOnHandler
   {
      IWriteRepository<TransferOn> repository;
      IEnumerable<IValidator<TransferOn>> validators;

      public TransferOnHandler(IWriteRepository<TransferOn> repository
         , IEnumerable<IValidator<TransferOn>> validators)
      {
         this.repository = repository;
         this.validators = validators;
      }

      public Either<Error, Unit> Handle(TransferOn request)
         => validators
            .Validate(request)
            .Bind(repository.Save);
   }
}