using System;
using Boc.Commands;
using LaYumba.Functional;
using Examples.Chapter1.DbLogger;
using Dapper;

namespace Boc.ValidImpl.Services
{
   using static F;
   using static ConnectionHelper;

   public interface IWriteRepository<T>
   {
      Exceptional<Unit> Save(T entity);
   }

   public class TransferOnWriteRepository : IWriteRepository<TransferOn>
   {
      string connString;

      public Exceptional<Unit> Save(TransferOn transfer)
      {
         try
         {
            ConnectionHelper.Connect(connString
               , c => c.Execute("INSERT ...", transfer));
         }
         catch (Exception ex) { return ex; }
         return Unit();
      }
   }
}
