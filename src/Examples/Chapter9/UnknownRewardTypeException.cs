using System;

namespace Examples.Chapter10.Data
{
   [Serializable]
   internal class UnknownRewardTypeException : Exception
   {
      public UnknownRewardTypeException()
      {
      }

      public UnknownRewardTypeException(string message) : base(message)
      {
      }

      public UnknownRewardTypeException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}