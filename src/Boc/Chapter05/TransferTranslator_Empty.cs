using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LaYumba.Functional;

namespace Boc.Services.Chapter4
{
   using Commands;
   using static F;

   public class TransferTranslator_Empty : IHandler<Transfer>
   {
      IValidator<Transfer> validator;
      IPublisher publisher;

      public void Handle(Transfer request)
      {
         throw new NotImplementedException("How would you implement this?");
      }
   }
}
