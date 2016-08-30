using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Internal;
using System;
using Microsoft.Extensions.Configuration;
using Boc.Domain;
using System.Linq;
using Boc.Commands;
using Boc.ValidImpl.Services;
using LaYumba.Functional;
using System.Collections.Generic;
using Examples.Chapter7;
using Boc.Chapter7.FunctionsEverywhere;

namespace Boc.Chapter7.Delegate
{
   public class ControllerActivator : IControllerActivator
   {
      public static ILoggerFactory loggerFactory { get; set; }
      DefaultControllerActivator defaultActivator;
      TypeActivatorCache typeActivatorCache;
      IConfigurationRoot configuration;

      public ControllerActivator(IConfigurationRoot configuration)
      {
         typeActivatorCache = new TypeActivatorCache();
         defaultActivator = new DefaultControllerActivator(typeActivatorCache);
         this.configuration = configuration;
      }

      public object Create(ControllerContext context)
      {
         var type = context.ActionDescriptor.ControllerTypeInfo;
         if (type.AsType().Equals(typeof(TransfersController)))
            return ConfigureTransferOnsController(context.HttpContext.RequestServices);

         return defaultActivator.Create(context);
      }

      TransfersController ConfigureTransferOnsController(IServiceProvider serviceProvider)
      {
         // persistence layer
         ConnectionString connString = configuration.GetSection("ConnectionString").Value;
         var persist = Sql.TryExecute.Apply(connString).Apply(Sql.Queries.InsertTransferOn);

         // service layer
         var validators = ConfigureTransferOnValidators();
         var handle = TransferOnHandler.Handle.Apply(validators).Apply(persist);

         // api layer
         var logger = loggerFactory.CreateLogger<TransfersController>();
         return new TransfersController(logger, handle);
      }

      IEnumerable<IValidator<TransferOn>> ConfigureTransferOnValidators()
         => Enumerable.Empty<IValidator<TransferOn>>();

      public void Release(ControllerContext context, object controller)
      {
         var disposable = controller as IDisposable;
         if (disposable != null) disposable.Dispose();
      }
   }

   public class UseCaseFactory
   {
      ILoggerFactory loggerFactory;
      IConfigurationRoot configuration;

      public UseCaseFactory(IConfigurationRoot configuration
         , ILoggerFactory loggerFactory)
      {
         this.loggerFactory = loggerFactory;
         this.configuration = configuration;
      }

      //public Func<TransferOn, IActionResult> PersistTransferOn(ControllerContext context)
      //{
      //   // can get other dependencies from here...
      //   IServiceProvider serviceProvider = context.HttpContext.RequestServices;
      //}

      public Func<TransferOn, IActionResult> PersistTransferOn()
      {
         // persistence layer
         ConnectionString connString = configuration.GetSection("ConnectionString").Value;
         var persist = Sql.TryExecute
            .Apply(connString)
            .Apply(Sql.Queries.InsertTransferOn);

         // service layer
         var validators = ConfigureTransferOnValidators();
         var handle = TransferOnHandler.Handle
            .Apply(validators)
            .Apply(persist);

         // api layer
         var logger = loggerFactory.CreateLogger<TransfersController>();
         return UseCases.TransferOn
            .Apply(logger)
            .Apply(handle);
      }

      IEnumerable<IValidator<TransferOn>> ConfigureTransferOnValidators()
         => Enumerable.Empty<IValidator<TransferOn>>();

      public void Release(ControllerContext context, object controller)
      {
         var disposable = controller as IDisposable;
         if (disposable != null) disposable.Dispose();
      }
   }
}
