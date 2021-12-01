using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class DebitAccount : IAccount
    {
        private readonly Guid _id;
        public DebitAccount(decimal money, int limit)
        {
            DebitLimit = limit;
            Money = money;
            _id = Guid.NewGuid();
            AccountAge = 0;
        }

        public int DebitLimit { get; private set; }

        public int AccountAge { get; private set; }
        public decimal Money { get; private set; }
        public void Put(decimal sum)
        {
            Money += sum;
        }

        public void Withdraw(decimal sum)
        {
            if (Money - sum < -DebitLimit)
                throw new BankException("Can't go below Debit Limit");
            Money -= sum;
        }

        public void Transfer(decimal sum, IAccount targetAccount)
        {
            if (Money - sum < -DebitLimit)
                throw new BankException("Can't go below Debit Limit");
            Money -= sum;
            targetAccount.Put(sum);
        }

        public AccountType GetAccountType()
        {
            return AccountType.DebitAccount;
        }

        public void PayComission(decimal percentage)
        {
            // death
        }
    }
}