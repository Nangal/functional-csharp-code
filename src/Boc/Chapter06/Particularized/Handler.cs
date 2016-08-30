using LaYumba.Functional;
using System.Collections.Generic;
using Boc.Commands;

namespace Boc.ValidImpl.Services
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

      public Validation<Exceptional<Unit>> Handle(TransferOn request)
         => validators
            .Validate(request)
            .Map(repository.Save);
   }
}
