using System;

namespace Boc.Commands
{
   public enum PaymentFrequency
   {
      Once, Daily, Weekly, Monthly, Annual
   }

   public class Transfer : Command
   {
      public Guid DebitedAccountId { get; set; }

      public string Beneficiary { get; set; }
      public string Iban { get; set; }
      public string Bic { get; set; }

      public DateTime Date { get; set; }
      public Money Amount { get; set; }
      public string Reference { get; set; }

      public PaymentFrequency Frequency { get; set; }
      public DateTime Finalization { get; set; }
   }

   // a transfer to be carried out immediately
   public class TransferNow : Transfer
   {
      internal TransferNow WithTimestamp(DateTime utcNow)
      {
         throw new NotImplementedException();
      }
   }

   // a transfer to be carried out at a future date
   public class TransferOn : Transfer
      , IEntity // so that it can be persisted with IRepository
   {
      public Guid Id { get; private set; }
   }

   // a transfer that recurs at regular intervale
   public class SetupStandingOrder : Transfer, IEntity
   {
      public Guid Id { get; private set; }
   }
}