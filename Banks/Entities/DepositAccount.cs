using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class DepositAccount : IAccount
    {
        private readonly Guid _id;
        public DepositAccount(decimal money, int accountTerm)
        {
            AccountTerm = accountTerm;
            Money = money;

            // RemainderPercentage = 0;
            _id = Guid.NewGuid();
            AccountAge = 0;
        }

        public int AccountAge { get; private set; }
        public int AccountTerm { get; private set; }
        public decimal Money { get; private set; }

        // public decimal RemainderPercentage { get; private set; }
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

        public AccountType GetAccountType()
        {
            return AccountType.DepositAccount;
        }

        public void AddRemainderPercentage(decimal percentage)
        {
            // misery
        }// trash
    }
}