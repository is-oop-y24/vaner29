using System;
using System.Runtime.InteropServices.ComTypes;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class DebitAccount : IAccount
    {
        public DebitAccount(decimal money, int percentage, Guid clientId)
        {
            Money = money;
            ClientId = clientId;
            Percentage = percentage;
        }

        public Guid ClientId { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();
        public int Percentage { get; private set; }
        public decimal Remainder { get; private set; } = 0;
        public int AccountAge { get; private set; } = 0;
        public decimal Money { get; private set; }
        public void Put(decimal sum)
        {
            Money += sum;
        }

        public void Withdraw(decimal sum)
        {
            if (Money - sum < 0)
                throw new BankException("Not enough money in the account");
            Money -= sum;
        }

        public void Transfer(decimal sum, IAccount targetAccount)
        {
            if (Money - sum < 0)
                throw new BankException("Not enough money in the account");
            Withdraw(sum);
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
                Remainder += Money * Percentage / 100 / 365;
                days--;
                if (AccountAge % 30 == 0)
                {
                    Money += Remainder;
                    Remainder = 0;
                }
            }
        }

        public decimal GetCurrentMoney()
        {
            return Money;
        }

        public int GetAccountAge()
        {
            return AccountAge;
        }

        public string GetAccountType()
        {
            return "Debit Account";
        }

        public void UpdatePercentage(int newPercentage)
        {
            Percentage = newPercentage;
        }
    }
}