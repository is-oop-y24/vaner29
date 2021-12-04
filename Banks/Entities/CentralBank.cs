using System;
using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBank
    {
        private List<Bank> _banks = new List<Bank>();
        public IReadOnlyList<Bank> Banks => _banks;
        public Bank AddBank(BankSettings settings)
        {
            var bank = new Bank(settings);
            _banks.Add(bank);
            return bank;
        }

        public Bank GetBankById(Guid id)
        {
            if (_banks.Find(x => x.Id == id) == null)
                throw new BankException("Couldn't find a bank with such Id");
            return _banks.Find(x => x.Id == id);
        }

        public void TransferBetweenBanks(Guid bankIdOut, Guid bankIdIn, decimal sum, Guid accountIdOut, Guid accountIdIn)
        {
            GetBankById(bankIdOut).TransferBetweenBanks(
                accountIdOut, GetBankById(bankIdIn).GetAccountById(accountIdIn), sum);
        }

        public void IncrementDays(int days)
        {
            foreach (var bank in Banks)
            {
                bank.IncrementDays(days);
            }
        }
    }
}