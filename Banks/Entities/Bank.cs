using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Tools;

namespace Banks.Entities
{
    public class Bank
    {
        private List<Client> _clients = new List<Client>();
        private List<IAccount> _accounts = new List<IAccount>();
        private List<ITransaction> _transactions = new List<ITransaction>();

        public Bank(BankSettings settings)
        {
            if (settings == null)
                throw new BankException("Settings can't be null");
            Settings = settings;
        }

        public Bank(Bank bank)
        {
            Settings = bank.Settings;
            Id = bank.Id;
        }

        public IReadOnlyList<ITransaction> Transactions => _transactions;
        public IReadOnlyList<IAccount> Accounts => _accounts;

        public Guid Id { get; private set; } = Guid.NewGuid();
        public BankSettings Settings { get; private set; }

        public void AddClient(Client newClient)
        {
            _clients.Add(newClient);
        }

        public bool SusCheck(Guid id)
        {
            Client client = _clients.Find(x => x.Id == id);
            if (client == null)
                throw new BankException("couldn't find a client with this Id");
            if ((Settings.AddressNeeded && client.Address == "none") || (Settings.PassportNeeded && client.PassportId == 0))
                return true;
            return false;
        }

        public IAccount GetAccountById(Guid id)
        {
            return _accounts.Find(x => x.GetAccountId() == id);
        }

        public void AddDebitAccount(Guid clientId, decimal sum)
        {
            _accounts.Add(new DebitAccount(sum, Settings.DebitRemainderPercentage, clientId));
        }

        public int GetPercentageForDeposit(decimal sum)
        {
            return (from range in Settings.DepositPercentageRanges where sum >= range.Min && sum <= range.Max select range.Percentage).FirstOrDefault();
        }

        public void AddDepositAccount(Guid clientId, decimal sum)
        {
            int percentage = GetPercentageForDeposit(sum);
            _accounts.Add(new DepositAccount(sum, Settings.DepositTerm, percentage, clientId));
        }

        public void AddCreditAccount(Guid clientId, decimal sum)
        {
            _accounts.Add(new CreditAccount(sum, Settings.CreditCommissionPercentage, Settings.CreditLimit, clientId));
        }

        public void Put(Guid id, decimal sum)
        {
            if (GetAccountById(id) == null)
                throw new BankException("Coudln't find an account with that Id");
            if (SusCheck(GetAccountById(id).GetClientId()) && sum > Settings.SusLimit)
                throw new BankException("Put Sum can't be this high for sus accounts");
            GetAccountById(id).Put(sum);
            _transactions.Add(new TransactionPut(Id,  GetAccountById(id), sum));
        }

        public void Withdraw(Guid id, decimal sum)
        {
            if (GetAccountById(id) == null)
                throw new BankException("Coudln't find an account with that Id");
            if (SusCheck(GetAccountById(id).GetClientId()) && sum > Settings.SusLimit)
                throw new BankException("Withdrawal Sum can't be this high for sus accounts");
            GetAccountById(id).Withdraw(sum);
            _transactions.Add(new TransactionWithdraw(Id,  GetAccountById(id), sum));
        }

        public void Transfer(Guid outId, Guid inId, decimal sum)
        {
            if (GetAccountById(outId) == null)
                throw new BankException("Coudln't find an output account with that Id");
            if (GetAccountById(inId) == null)
                throw new BankException("Coudln't find an input account with that Id");
            if (SusCheck(GetAccountById(outId).GetClientId()) && sum > Settings.SusLimit)
                throw new BankException("Transfer sum can't be this high for sus accounts");

            if (SusCheck(GetAccountById(inId).GetClientId()) && sum > Settings.SusLimit)
                throw new BankException("Transfer sum can't be this high for sus accounts");
            GetAccountById(outId).Transfer(sum, GetAccountById(inId));
            _transactions.Add(new TransactionTransfer(Id,  GetAccountById(outId),  GetAccountById(inId), sum));
        }

        public void TransferBetweenBanks(Guid outId, IAccount inAccount, decimal sum)
        {
            if (SusCheck(GetAccountById(outId).GetClientId()) && sum > Settings.SusLimit)
                throw new BankException("Transfer sum can't be this high for sus accounts");
            GetAccountById(outId).Transfer(sum, inAccount);
            _transactions.Add(new TransactionTransfer(Id,  GetAccountById(outId), inAccount, sum));
        }

        public void IncrementDays(int days)
        {
            foreach (var account in _accounts)
            {
                account.IncrementDays(days);
            }
        }

        public void CancelTransaction(Guid id)
        {
            if (_transactions.Find(x => x.GetId() == id) == null)
                throw new BankException("couldn't find a transaction with that Id");
            _transactions.Find(x => x.GetId() == id).CancelTransaction();
        }

        public List<IAccount> GetAllClientAccounts(Guid id)
        {
            return _accounts.Where(account => account.GetClientId() == id).ToList();
        }

        public List<ITransaction> GetAllClientTransactions(Guid id)
        {
            var trans = new List<ITransaction>();
            foreach (var account in GetAllClientAccounts(id))
            {
                foreach (var transaction in _transactions)
                {
                    if (account.GetAccountId() == transaction.GetClientAccounts()[0].GetAccountId() || account.GetAccountId() == transaction.GetClientAccounts()[1].GetAccountId())
                        trans.Add(transaction);
                }
            }

            return trans;
        }

        public void NotifySubscribers(string notification)
        {
            foreach (var client in _clients.Where(client => client.IsSubbed))
            {
                client.AddToLog(notification);
            }
        }

        public void UpdateName(string newName)
        {
            Settings.UpdateName(newName);
            NotifySubscribers("Bank name was updated");
        }

        public void UpdatePassportNeed(bool newNeed)
        {
            Settings.UpdatePassportNeed(newNeed);
            NotifySubscribers("The passport requirement was updated");
        }

        public void UpdateAddressNeed(bool newNeed)
        {
            Settings.UpdateAddressNeed(newNeed);
            NotifySubscribers("The Address requirement was updated");
        }

        public void UpdateSusLimit(decimal newLimit)
        {
            Settings.UpdateSusLimit(newLimit);
            NotifySubscribers("Transaction Limit was updated");
        }

        public void UpdateDebitPercentage(int newPercentage)
        {
            Settings.UpdateDebitPercentage(newPercentage);
            NotifySubscribers("Debit Percentage was updated");
        }

        public void UpdateCreditLimit(decimal newLimit)
        {
            Settings.UpdateCreditLimit(newLimit);
            NotifySubscribers("Credit Limit was updated");
        }

        public void UpdateCreditPercentage(int newPercentage)
        {
            Settings.UpdateCreditPercentage(newPercentage);
            NotifySubscribers("Credit Comission was updated");
        }

        public void UpdateDepositTerm(int newTerm)
        {
            Settings.UpdateDepositTerm(newTerm);
            NotifySubscribers("Deposit term was updated");
        }

        public void UpdateDepositRanges(List<DepositPercentageRange> newRanges)
        {
            Settings.UpdateDepositRanges(newRanges);
            NotifySubscribers("Deposit Percentage Ranges was updated");
        }

        public Client GetClientById(Guid id)
        {
            return _clients.Find(x => x.Id == id);
        }
    }
}