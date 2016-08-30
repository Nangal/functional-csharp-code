using LaYumba.Functional;
using System.Collections.Generic;
using Boc.Commands;
using Boc.Domain;
using System;
using Boc.ValidImpl.Services;
using NUnit.Framework;
using System.Linq;

namespace Boc.Chapter7
{
   namespace Classic
   {
      public interface IHandler<T>
      {
         Validation<Exceptional<Unit>> Handle(T cmd);
      }

      public class TransferOnHandler : IHandler<TransferOn>
      {
         IWriteRepository<TransferOn> repository;
         IEnumerable<IValidator<TransferOn>> validators;

         public TransferOnHandler(IWriteRepository<TransferOn> repository
            , IEnumerable<IValidator<TransferOn>> validators)
         {
            this.repository = repository;
            this.validators = validators;
         }

         public Validation<Exceptional<Unit>> Handle(TransferOn request)
            => validators
               .Validate(request)
               .Map(repository.Save);
      }
   }

   namespace Delegate
   {
      using static F;

      public static class TransferOnHandler
      {
         public static Func<IEnumerable<IValidator<TransferOn>>
                           , Func<TransferOn, Exceptional<Unit>>
                           , TransferOn
                           , Validation<Exceptional<Unit>>>
         Handle => (validators, save, cmd)
            => validators.Validate(cmd).Map(save);
      }

      
      public class TransferOnHandler_Test
      {
         [Test]
         public void WhenPersistOk_AndNoValidators_ThenOk()
         {
            var uut = TransferOnHandler.Handle;
            var result = uut(Enumerable.Empty<IValidator<TransferOn>>()
               , _ => Exceptional.Of(Unit())
               , new TransferOn());

            Assert.IsTrue(result.IsValid);
            result.ForEach(ex => Assert.IsTrue(ex.Success));
         }
      }
   }
}