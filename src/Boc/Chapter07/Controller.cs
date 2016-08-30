using Boc.Commands;
using Boc.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using LaYumba.Functional;

namespace Boc.Chapter7
{
   // option 1. use MVC, and inject the IHandler as a dependency
   namespace Classic
   {
      public class TransfersController : Controller
      {
         ILogger<TransfersController> logger;
         IHandler<TransferOn> service;

         public TransfersController(ILogger<TransfersController> logger
            , TransferOnHandler service)
         {
            this.logger = logger;
            this.service = service;
         }

         //[HttpPut, Route("api/TransferOns")]
         public IActionResult TransferOn([FromBody] TransferOn cmd)
            => service.Handle(cmd).Match(
               Invalid: BadRequest,
               Valid: result => result.Match(
                  Exception: OnFaulted,
                  Success: _ => Ok()));

         public IActionResult OnFaulted(Exception ex)
         {
            logger.LogError(ex.Message);
            return StatusCode(500, Errors.UnexpectedError);
         }
      }
   }

   // option 2. use MVC, and inject a handler function as a dependency
   namespace Delegate
   {
      public class TransfersController : Controller
      {
         ILogger<TransfersController> logger;
         Func<TransferOn, Validation<Exceptional<Unit>>> handle;

         public TransfersController(ILogger<TransfersController> logger
            , Func<TransferOn, Validation<Exceptional<Unit>>> handle)
         {
            this.logger = logger;
            this.handle = handle; // just inject the function we need
         }

         [HttpPut, Route("api/TransferOn")]
         public IActionResult TransferOn([FromBody] TransferOn TransferOn)
            => handle(TransferOn).Match( // invoke the handler function
               Invalid: BadRequest,
               Valid: result => result.Match(
                  Exception: OnFaulted,
                  Success: _ => Ok()));

         public IActionResult OnFaulted(Exception ex)
         {
            logger.LogError(ex.Message);
            return StatusCode(500, Errors.UnexpectedError);
         }
      }
   }

   // option 3. dont use MVC; just use a function, and inject it into 
   // the pipeline in the Startup class
   namespace FunctionsEverywhere
   {
      using static ActionResultFactory;

      public class UseCases
      {
         public static Func<ILogger
            , Func<TransferOn, Validation<Exceptional<Unit>>>
            , TransferOn
            , IActionResult>
         TransferOn => (logger, handle, cmd) =>
         {
            Func<Exception, IActionResult> onFaulted = ex =>
            {
               logger.LogError(ex.Message);
               return InternalServerError(ex);
            };

            return handle(cmd).Match(
               Invalid: BadRequest,
               Valid: result => result.Match(
                  Exception: onFaulted,
                  Success: _ => Ok()));
         };
      }

      static class ActionResultFactory
      {
         public static IActionResult Ok() => new OkResult();
         public static IActionResult Ok(object value) => new OkObjectResult(value);
         public static IActionResult BadRequest(object error) => new BadRequestObjectResult(error);
         public static IActionResult InternalServerError(object value)
         {
            var result = new ObjectResult(value);
            result.StatusCode = 500;
            return result;
         }
      }
   }
}
