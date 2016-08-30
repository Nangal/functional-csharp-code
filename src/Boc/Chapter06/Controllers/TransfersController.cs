using Boc.Commands;
using Boc.EitherImpl.Services.InjectedValidators;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;

namespace Boc.Api
{
   public class TransfersController : Controller
   {
      private TransferOnHandler transfers;

      [HttpPost, Route("api/transfers/future")]
      public IActionResult BookTransferOn_v1([FromBody] TransferOn request)
         => transfers.Handle(request).Match<IActionResult>(
            Right: _ => Ok(),
            Left: BadRequest);

      [HttpPost, Route("api/transfers/future")]
      public ResultDto<Unit> BookTransferOn_v2([FromBody] TransferOn request)
         => transfers.Handle(request).ToResult();
   }
}
