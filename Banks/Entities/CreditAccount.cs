using System;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class CreditAccount : IAccount
    {
        public CreditAccount(decimal money, int percentage, decimal limit, Guid clientId)
        {
            ClientId = clientId;
            CreditLimit = limit;
            Money = money;
            CommissionPercentage = percentage;
        }

        public Guid ClientId { get; private set; }
        public Guid Id { get; private set; } = Guid.NewGuid();
        public decimal CreditLimit { get; private set; }
        public int CommissionPercentage { get; private set; }
        public int AccountAge { get; private set; } = 0;
        public decimal Money { get; private set; }
        public void Put(decimal sum)
        {
            Money += sum;
        }

        public void Withdraw(decimal sum)
        {
            if (Money - sum < -CreditLimit)
                    throw new BankException("Can't go below the credit limit");
            Money -= sum;
        }

        public void Transfer(decimal sum, IAccount targetAccount)
        {
            if (Money - sum < -CreditLimit)
                    throw new BankException("Can't go below the credit limit");
            Money -= sum;
            targetAccount.Put(sum);
        }

        public Guid GetAccountId()
        {
            return Id;
        }

        public Guid GetClientId()
        {
            return ClientId;
        }

        public void IncrementDays(int days)
        {
            while (days > 0)
            {
                AccountAge++;
                days--;
                if (AccountAge % 30 == 0 && Money < 0)
                {
                    Money -= Money * CommissionPercentage / 100;
                }
            }
        }

        public decimal GetCurrentMoney()
        {
            return Money;
        }
    }
}