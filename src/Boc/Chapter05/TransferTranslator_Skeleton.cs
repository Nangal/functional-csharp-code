using System;
using System.Collections.Generic;
using LaYumba.Functional;

namespace Boc.Services.Chapter4
{
   using Commands;
   using static F;

   public class TransferTranslator_Skeleton : IHandler<Transfer>
   {
      IValidator<Transfer> validator;
      IPublisher publisher;

      public void Handle(Transfer request)
         => Some(request)                // Option<Transfer>
            .Where(validator.IsValid)   // Option<Transfer>
            .AsEnumerable()              // IEnumerable<Transfer>
            .Bind(ToCommands)            // IEnumerable<Command>
            .ForEach(publisher.Publish); // Unit

      public void Handle_1(Transfer request)
         => Some(request)                // Option<Transfer>
            .Where(validator.IsValid)   // Option<Transfer>
            .Map(ToCommands)             // Option<IEnumerable<Transfer>>
            .ForEach(commands => commands.ForEach(publisher.Publish));

      private IEnumerable<Command> ToCommands(Transfer request)
      {
         throw new NotImplementedException("need to figure this out");
      }
   }
}
