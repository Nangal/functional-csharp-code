using System;
using Boc.Commands;
using Boc.Services;
using NSubstitute;
using NUnit.Framework;
using SimpleInjector;
using Boc.Services.Chapter4;

namespace Boc.Tests
{
   
   public class TransferTranslatorTest
   {
      TransferTranslator sut;
      IDateTimeService calendar = Substitute.For<IDateTimeService>();
      IValidator<Transfer> validator = Substitute.For<IValidator<Transfer>>();
      IPublisher publisher = Substitute.For<IPublisher>();

      [SetUp]
      public void SetUp()
      {
         var container = new Container();
         container.RegisterSingleton(validator);
         container.RegisterSingleton(calendar);
         container.RegisterSingleton(publisher);

         validator.IsValid(Arg.Any<Transfer>()).Returns(true);
         sut = container.GetInstance<TransferTranslator>();
      }

      [TestCase(false, PaymentFrequency.Once,    0, 1, 0)]
      [TestCase(false, PaymentFrequency.Monthly, 0, 0, 1)]
      [TestCase(true,  PaymentFrequency.Once,    1, 0, 0)]
      [TestCase(true,  PaymentFrequency.Monthly, 1, 0, 1)]
      public void ItPublishesTheCorrectCommands
         ( bool isToday, PaymentFrequency frequency // inputs
         , int immediate, int future, int standing) // expectations
      {
         calendar.IsToday(Arg.Any<DateTime>()).Returns(isToday);
         var transfer = new Transfer { Frequency = frequency };

         sut.Handle(transfer);

         publisher.Received(immediate).Publish(Arg.Any<TransferNow>());
         publisher.Received(future).Publish(Arg.Any<TransferOn>());
         publisher.Received(standing).Publish(Arg.Any<SetupStandingOrder>());
      }

      [TearDown]
      public void TearDown() => publisher.ClearReceivedCalls();
   }
}