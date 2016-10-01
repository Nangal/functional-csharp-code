using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using Examples.Domain;

namespace Examples.Chapter10.Data.Account.Immutable
{
   public sealed class AccountState
   {
      public AccountStatus Status { get; }
      public CurrencyCode Currency { get; }
      public decimal AllowedOverdraft { get; }
      public IEnumerable<Transaction> TransactionHistory { get; }

      public AccountState(CurrencyCode Currency
         , AccountStatus Status = AccountStatus.Requested
         , decimal AllowedOverdraft = 0
         , IEnumerable<Transaction> Transactions = null)
      {
         this.Status = Status;
         this.Currency = Currency;
         this.AllowedOverdraft = AllowedOverdraft;
         this.TransactionHistory = ImmutableList.CreateRange
            (Transactions ?? Enumerable.Empty<Transaction>());
      }

      public AccountState Add(Transaction t)
         => new AccountState(
               Transactions: TransactionHistory.Prepend(t),
               Currency: this.Currency,
               Status: this.Status,
               AllowedOverdraft: this.AllowedOverdraft
            );

      public AccountState WithStatus(AccountStatus newStatus)
         => new AccountState(
               Status: newStatus,
               Currency: this.Currency,
               AllowedOverdraft: this.AllowedOverdraft,
               Transactions: this.TransactionHistory
            );
   }
   class Usage
   {
      void _main()
      {
         var account = new AccountState
         (
            Currency: "EUR",
            Status: AccountStatus.Active
         );
         var newState = account.WithStatus(AccountStatus.Frozen);
      }
   }
}