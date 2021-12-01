using System;

namespace Banks.Entities
{
    public interface IAccount
    {
        void Put(decimal sum);
        void Withdraw(decimal sum);
        void Transfer(decimal sum, IAccount targetAccount);

        // Guid GetAccountId();
        AccountType GetAccountType();
    }
}