using System;

namespace Boc.Commands
{
   public abstract class Command
   {
      public Guid RequestId { get; }
      public string UserId { get; }
      public DateTime Timestamp { get; private set; }

      public static T WithTimestamp<T>(DateTime timestamp, T command) where T : Command
      {
         var result = (T)command.MemberwiseClone();
         result.Timestamp = timestamp;
         return result;
      }
   }
}