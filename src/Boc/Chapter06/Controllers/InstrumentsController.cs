﻿using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;

namespace Boc.Api
{
   public class InstrumentsController : Controller
   {
      private IInstrumentService instruments;

      [HttpGet, Route("api/ping")]
      public IActionResult Ping()
         => Ok("Pong");

      [HttpGet, Route("api/instruments/{ticker}/details")]
      public IActionResult GetAccountDetails(string ticker)
         => instruments.GetInstrumentDetails(ticker).Match<IActionResult>(
            Some: Ok,
            None: NotFound);
   }

   public interface IInstrumentService
   {
      Option<InstrumentDetails> GetInstrumentDetails(string ticker);
   }

   public class InstrumentDetails { }
}
