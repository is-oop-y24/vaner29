using System;
using System.Collections.Generic;
using Banks.Interfaces;

namespace Banks.Entities
{
    public class Bank
    {
        private IReadOnlyDictionary<Client, List<IAccount>> _clientsAndAccounts;
        public Bank(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
            _clientsAndAccounts = new Dictionary<Client, List<IAccount>>();
        }

        public string Name { get; private set; }
        public Guid Id { get; private set; }
        public int CreditRemainderPercentage { get; private set; }
    }
}