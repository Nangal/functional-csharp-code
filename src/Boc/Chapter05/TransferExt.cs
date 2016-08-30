namespace Boc.Commands
{
   public static class TransferExt
   {
      public static TransferNow ToTransferNow(this Transfer request)
      {
         return new TransferNow();
      }

      public static TransferOn ToTransferOn(this Transfer request)
      {
         return new TransferOn();
      }

      public static SetupStandingOrder ToSetupStandingOrder(this Transfer request)
      {
         return new SetupStandingOrder();
      }
   }
}