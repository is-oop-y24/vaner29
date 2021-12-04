using System;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class DepositAccount : IAccount
    {
        public DepositAccount(decimal money, int accountTerm, int percentage, Guid clientId)
        {
            ClientId = clientId;
            AccountTerm = accountTerm;
            Money = money;
            Percentage = percentage;
        }

        public Guid ClientId { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();
        public int AccountAge { get; private set; } = 0;
        public int AccountTerm { get; private set; }
        public decimal Remainder { get; private set; } = 0;
        public int Percentage { get; private set; }
        public decimal Money { get; private set; }
        public void Put(decimal sum)
        {
            Money += sum;
        }

        public void Withdraw(decimal sum)
        {
            if (AccountAge < AccountTerm)
                throw new BankException("Can't withdraw until account term is over");
            if (Money < sum)
                throw new BankException("Not enough money in the bank account");
            Money -= sum;
        }

        public void Transfer(decimal sum, IAccount targetAccount)
        {
            if (AccountAge < AccountTerm)
                throw new BankException("Can't withdraw until account term is over");
            if (Money < sum)
                throw new BankException("Not enough money in the bank account");
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

        public void UpdatePercentage(int newPercentage)
        {
            Percentage = newPercentage;
        }

        public void IncrementDays(int days)
        {
            while (days > 0 && AccountAge <= AccountTerm)
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
            return "Deposit Account";
        }
    }
}