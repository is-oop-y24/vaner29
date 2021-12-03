using System;

namespace Banks.Interfaces
{
    public interface IAccount
    {
        void Put(decimal sum);
        void Withdraw(decimal sum);
        void Transfer(decimal sum, IAccount targetAccount);
        Guid GetAccountId();
        /* (int GetAccountAge();
        int GetYearlyPercentage();
        decimal GetCurrentMoney();
        decimal GetCurrentRemainder(); */
    }
}