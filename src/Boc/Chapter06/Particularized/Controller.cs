using Boc.ValidImpl.Services;
using Boc.Commands;
using Boc.Domain;
using Microsoft.AspNetCore.Mvc;
using Exception = System.Exception;
using Microsoft.Extensions.Logging;

namespace Boc.ValidImpl
{
   public class TransfersController : Controller
   {
      ILogger<TransfersController> logger;
      private TransferOnHandler transfers;

      [HttpPost, Route("api/transfers/future")]
      public IActionResult MakeFutureTransfer([FromBody] TransferOn request)
         => transfers.Handle(request).Match(
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
