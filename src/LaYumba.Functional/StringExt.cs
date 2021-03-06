﻿using System;

namespace LaYumba.Functional
{
   using static F;

   public static class StringExt
   {
      public static Option<int> ParseInt(this string s)
      {
         int result;
         return int.TryParse(s, out result)
            ? Some(result) : None;
      }

      public static int ParseInt(this string s, int @default)
         => s.ParseInt().GetOrElse(@default);

      public static Option<double> ParseDouble(this string s)
      {
         double d;
         return double.TryParse(s, out d) ? Some(d) : None;
      }

      public static Option<long> ParseLong(this string s)
      {
         long l;
         return long.TryParse(s, out l) ? Some(l) : None;
      }

      public static Option<DateTime> ParseDate(this string s)
      {
         DateTime d;
         return DateTime.TryParse(s, out d) ? Some(d) : None;
      }

      public static Option<T> ParseEnum<T>(this string s) where T : struct
      {
         T t;
         return Enum.TryParse(s, out t) ? Some(t) : None ;
      }
   }
}
