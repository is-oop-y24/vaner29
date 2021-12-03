using System;

namespace Banks.Interfaces
{
    public interface IAccount
    {
        void Put(decimal sum);
        void Withdraw(decimal sum);
        void Transfer(decimal sum, IAccount targetAccount);
        Guid GetAccountId();

        Guid GetClientId();

        void IncrementDays(int days);
        decimal GetCurrentMoney();
    }
}