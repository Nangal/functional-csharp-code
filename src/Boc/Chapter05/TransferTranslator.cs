using System;
using System.Collections.Generic;
using System.Linq;
using Boc.Commands;
using LaYumba.Functional;

namespace Boc.Services.Chapter4
{
   using static F;
   using TransferMappingRules = Dictionary<Predicate<Transfer>, 
                                           Func<Transfer, Command>>;

   public class TransferTranslator : IHandler<Transfer>
   {
      IValidator<Transfer> validator;
      IDateTimeService clock;
      IPublisher publisher;

      public TransferTranslator(IDateTimeService clock
          , IValidator<Transfer> validator
          , IPublisher publisher)
      {
         this.clock = clock;
         this.validator = validator;
         this.publisher = publisher;
      }

      TransferMappingRules Rules => new TransferMappingRules
      {
         [IsImmediate] = t => t.ToTransferNow(),
         [IsStandingOrder] = t => t.ToSetupStandingOrder(),
         [IsFuture] = t => t.ToTransferOn(),
      };

      bool IsStandingOrder(Transfer t) => t.Frequency != PaymentFrequency.Once;
      bool IsImmediate(Transfer t) => clock.IsToday(t.Date);
      bool IsFuture(Transfer t) => !IsStandingOrder(t) && !IsImmediate(t);

      public void Handle(Transfer request)
         => Some(request)                // Option<Transfer>
            .Where(validator.IsValid)   // Option<Transfer>
            .Bind(ToCommands)            // IEnumerable<Command>
            .ForEach(publisher.Publish); // Unit

      IEnumerable<Command> ToCommands(Transfer transfer)
         => Rules
            .Where(rule => rule.Key(transfer))
            .Map(rule => rule.Value(transfer));

      IEnumerable<Command> ToCommands_LINQ(Transfer transfer)
         => from rule in Rules
            where rule.Key(transfer)
            select rule.Value(transfer);
   }
}
   