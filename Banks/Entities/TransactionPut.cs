using System;
using System.Collections.Generic;
using System.Net;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionPut : ITransaction
    {
        public TransactionPut(Guid bankId, IAccount clientAccount, decimal moneyUsed)
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
            ClientAccounts[0].Withdraw(MoneyUsed);
        }

        public List<IAccount> GetClientAccounts()
        {
            return ClientAccounts;
        }
    }
}