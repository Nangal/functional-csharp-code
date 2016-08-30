using LaYumba.Functional;

namespace Boc.Domain
{
   public static class Errors
   {
      public static InsufficientBalanceError InsufficientBalance
         => new InsufficientBalanceError();

      public static InvalidBicError InvalidBic
         => new InvalidBicError();

      public static CannotActivateClosedAccountError CannotActivateClosedAccount
         => new CannotActivateClosedAccountError();

      public static TransferDateIsPastError TransferDateIsPast 
         => new TransferDateIsPastError();

      public static AccountNotActiveError AccountNotActive
         => new AccountNotActiveError();

      public static object UnexpectedError { get; internal set; }
   }

   public sealed class AccountNotActiveError : Error
   {
      public override string Message { get; }
         = "The account is not active; the requested operation cannot be completed";
   }

   public sealed class InvalidBicError : Error
   {
      public override string Message { get; }
         = "The beneficiary's BIC/SWIFT code is invalid";
   }

   public sealed class InsufficientBalanceError : Error
   {
      public override string Message { get; }
         = "Insufficient funds to fulfil the requested operation";
   }

   public sealed class CannotActivateClosedAccountError : Error
   {
      public override string Message { get; }
         = "Cannot activate an account that has been closed";
   }

   public sealed class TransferDateIsPastError : Error
   {
      public override string Message { get; }
         = "Transfer date cannot be in the past";
   }
}
