using Boc.Commands;
using LaYumba.Functional;
using System;

namespace Boc.EitherImpl.Services.Skeleton
{
   public class TransferOnHandler
   {
      IWriteRepository<TransferOn> repository;

      public Either<Error, Unit> Handle(TransferOn request)
         => Validate(request)
            .Bind(repository.Save);

      Either<Error, TransferOn> Validate(TransferOn request)
      {
         throw new NotImplementedException();
      }
   }
}