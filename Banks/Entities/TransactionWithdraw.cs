using System;
using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionWithdraw : ITransaction
    {
        public TransactionWithdraw(Guid bankId, IAccount clientAccount, decimal moneyUsed)
        {
            MoneyUsed = moneyUsed;
            BankId = bankId;
            ClientAccounts.Add(clientAccount);
        }

        public Guid BankId { get; }
        public Guid TransactionId { get; } = Guid.NewGuid();
        public decimal MoneyUsed { get; private set; }
        public List<IAccount> ClientAccounts { get; private set; } = new List<IAccount>();
        public bool WasCancelled { get; private set; } = false;
        public void CancelTransaction()
        {
            if (WasCancelled)
                throw new BankException("Can't cancel an operation twice");
            WasCancelled = true;
            ClientAccounts[0].Put(MoneyUsed);
        }

        public List<IAccount> GetClientAccounts()
        {
            return ClientAccounts;
        }

        public Guid GetId()
        {
            return TransactionId;
        }
    }
}