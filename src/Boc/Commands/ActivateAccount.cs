using System;

namespace Boc.Commands
{
   public class ActivateAccount : Command
   {
      public Guid AccountId { get; set; }
   }
}
