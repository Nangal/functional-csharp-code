using System;

namespace Examples.Chapter1.DbLogger
{
   public static class TimeSpanExt
   {
      public static TimeSpan Days(this int @this)
         => TimeSpan.FromDays(@this);

      public static DateTime Ago(this TimeSpan @this)
         => DateTime.UtcNow - @this;
   }
}