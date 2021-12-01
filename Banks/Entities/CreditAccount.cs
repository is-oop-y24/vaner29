using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class CreditAccount : IAccount
    {
        private readonly Guid _id;
        public CreditAccount(decimal money, int percentage)
        {
            Money = money;
            Percentage = percentage;

            // RemainderPercentage = 0;
            _id = Guid.NewGuid();
            AccountAge = 0;
        }

        public int Percentage { get; private set; }
        public int AccountAge { get; private set; }
        public decimal Money { get; private set; }

        // public decimal RemainderPercentage { get; private set; }
        public void Put(decimal sum)
        {
            Money += sum;
        }

        public void Withdraw(decimal sum)
        {
            if (Money < sum)
                throw new BankException("Not enough money in the bank account");
            Money -= sum;
        }

        public void Transfer(decimal sum, IAccount targetAccount)
        {
            if (Money < sum)
                throw new BankException("Not enough money in the bank account");
            Money -= sum;
            targetAccount.Put(sum);
        }

        public AccountType GetAccountType()
        {
            return AccountType.CreditAccount;
        }

        public void AddRemainderPercentage(decimal percentage)
        {
            // pain
        }// trash
    }
}