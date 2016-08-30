using System;
using LaYumba.Functional;
using Boc.Commands;

namespace Boc.Chapter7
{
   using static F;
   using Examples.Chapter7;

   public interface IWriteRepository<T>
   {
      Exceptional<Unit> Save(T entity);
   }
    
   namespace Classic
   {
      public class TransferOnWriteRepository : IWriteRepository<TransferOn>
      {
         ConnectionString conn;

         public TransferOnWriteRepository(ConnectionString conn)
         {
            this.conn = conn;
         }

         public Exceptional<Unit> Save(TransferOn transfer)
         {
            try { conn.Execute("INSERT ...", transfer); }
            catch (Exception ex) { return ex; }
            return Unit();
         }
      }
   }

   namespace Delegate // Boc.Data
   {
      public static class Sql
      {
         public static class Queries
         {
            public static readonly SqlTemplate InsertTransferOn = "INSERT ...";
         }

         public static Func< ConnectionString
                           , SqlTemplate
                           , object
                           , Exceptional<Unit> >
         TryExecute => (conn, sql, t) =>
         {
            try { conn.Execute(sql, t); }
            catch (Exception ex) { return ex; }
            return Unit();
         };
      }
   }
}
