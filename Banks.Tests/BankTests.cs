using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using System.Linq;
using Banks.Entities;
using Banks.Interfaces;

namespace Banks.Tests
{
    public class BankTests
    {
        private CentralBank _centralBank;
        private Bank _bank;
        private Bank _bank2;
        
        [SetUp]
        public void Setup()
        {
            _centralBank = new CentralBank();
            var settings = new BankSettings("Gaz", false, false, 10, 5, 100, 5, 61,
                new List<DepositPercentageRange> { new DepositPercentageRange(0, Decimal.MaxValue, 5) });
            _bank = _centralBank.AddBank(settings);
            _bank2 = _centralBank.AddBank(settings);
        }

        [Test]
        public void Create_Put_CheckResult()
        {
            var gleb = new Client("gleb", "grep");
            _bank.AddClient(gleb);
            _bank.AddDebitAccount(gleb.Id, 1);
            IAccount glebAcc = _bank.GetAllClientAccounts(gleb.Id)[0];
            Assert.AreEqual(1, glebAcc.GetCurrentMoney());
            _bank.Put(glebAcc.GetAccountId(),1);
            Assert.AreEqual(2, glebAcc.GetCurrentMoney());
        }
        [Test]
        public void Put_Cancel_ChecKResult()
        {
            var gleb = new Client("gleb", "grep");
            _bank.AddClient(gleb);
            _bank.AddDebitAccount(gleb.Id, 1);
            IAccount glebAcc = _bank.GetAllClientAccounts(gleb.Id)[0];
            _bank.Put(glebAcc.GetAccountId(),1);
            _bank.CancelTransaction(_bank.GetAllClientTransactions(gleb.Id)[0].GetId());
            Assert.AreEqual(1, glebAcc.GetCurrentMoney());
        }
        [Test]
        public void Withdraw_CheckResult()
        {
            var gleb = new Client("gleb", "grep");
            _bank.AddClient(gleb);
            _bank.AddDebitAccount(gleb.Id, 1);
            IAccount glebAcc = _bank.GetAllClientAccounts(gleb.Id)[0];
            _bank.Withdraw(glebAcc.GetAccountId(),1);
            Assert.AreEqual(0, glebAcc.GetCurrentMoney());
        }
        [Test]
        public void Withdraw_Cancel_CheckResult()
        {
            var gleb = new Client("gleb", "grep");
            _bank.AddClient(gleb);
            _bank.AddDebitAccount(gleb.Id, 1);
            IAccount glebAcc = _bank.GetAllClientAccounts(gleb.Id)[0];
            _bank.Withdraw(glebAcc.GetAccountId(),1);
            _bank.CancelTransaction(_bank.GetAllClientTransactions(gleb.Id)[0].GetId());
            Assert.AreEqual(1, glebAcc.GetCurrentMoney());
        }
        [Test]
        public void Transfer_CheckResult()
        {
            var gleb = new Client("gleb", "grep");
            var pudge = new Client("pudge", "dire");
            _bank.AddClient(gleb);
            _bank.AddClient(pudge);
            _bank.AddDebitAccount(gleb.Id, 1);
            _bank.AddDebitAccount(pudge.Id, 1);
            IAccount glebAcc = _bank.GetAllClientAccounts(gleb.Id)[0];
            IAccount pudgeAcc = _bank.GetAllClientAccounts(pudge.Id)[0];
            _bank.Transfer(glebAcc.GetAccountId(), pudgeAcc.GetAccountId(), 1);
            Assert.AreEqual(0, glebAcc.GetCurrentMoney());
            Assert.AreEqual(2, pudgeAcc.GetCurrentMoney());
        }
        [Test]
        public void Transfer_Cancel_CheckResult()
        {
            var gleb = new Client("gleb", "grep");
            var pudge = new Client("pudge", "dire");
            _bank.AddClient(gleb);
            _bank.AddClient(pudge);
            _bank.AddDebitAccount(gleb.Id, 1);
            _bank.AddDebitAccount(pudge.Id, 1);
            IAccount glebAcc = _bank.GetAllClientAccounts(gleb.Id)[0];
            IAccount pudgeAcc = _bank.GetAllClientAccounts(pudge.Id)[0];
            _bank.Transfer(glebAcc.GetAccountId(), pudgeAcc.GetAccountId(), 1);
            _bank.CancelTransaction(_bank.GetAllClientTransactions(pudge.Id)[0].GetId());
            Assert.AreEqual(1, glebAcc.GetCurrentMoney());
            Assert.AreEqual(1, pudgeAcc.GetCurrentMoney());
        }
        [Test]
        public void WaitForRemainderPercentage_CheckResult()
        {
            _bank.UpdateDebitPercentage(36500);
            var gleb = new Client("gleb", "grep");
            _bank.AddClient(gleb);
            _bank.AddDebitAccount(gleb.Id, 1);
            IAccount glebAcc = _bank.GetAllClientAccounts(gleb.Id)[0];
            _bank.IncrementDays(30);
            Assert.AreEqual(31, glebAcc.GetCurrentMoney());
        }
        [Test]
        public void ChangeSubscriptionStatus_ChangeBankSettings_CheckResult()
        {
            var gleb = new Client("gleb", "grep");
            _bank.AddClient(gleb);
            gleb.UpdateSubscription(true);
            _bank.UpdateDebitPercentage(36500);
            Assert.AreEqual("Debit Percentage was updated", gleb.NewsLogs[0]);
        }
        [Test]
        public void CreateBankInCentral_CheckIClientsAreAlright()
        {
            var gleb = new Client("gleb", "grep");
            var pudge = new Client("pudge", "dire");
            _centralBank.GetBankById(_bank.Id).AddClient(gleb);
            _bank.AddDebitAccount(gleb.Id, 1);
            IAccount glebAcc = _centralBank.GetBankById(_bank.Id).GetAllClientAccounts(gleb.Id)[0];
            _centralBank.GetBankById(_bank2.Id).AddClient(pudge);
            _bank2.AddDebitAccount(pudge.Id, 1);
            IAccount pudgeAcc = _centralBank.GetBankById(_bank2.Id).GetAllClientAccounts(pudge.Id)[0];
            Assert.AreEqual(1, glebAcc.GetCurrentMoney());
            Assert.AreEqual(1, pudgeAcc.GetCurrentMoney());
        }
        [Test]
        public void TransferBetweenBanks_CheckResults()
        {
            var gleb = new Client("gleb", "grep");
            var pudge = new Client("pudge", "dire");
            _centralBank.GetBankById(_bank.Id).AddClient(gleb);
            _bank.AddDebitAccount(gleb.Id, 1);
            IAccount glebAcc = _centralBank.GetBankById(_bank.Id).GetAllClientAccounts(gleb.Id)[0];
            _centralBank.GetBankById(_bank2.Id).AddClient(pudge);
            _bank2.AddDebitAccount(pudge.Id, 1);
            IAccount pudgeAcc = _centralBank.GetBankById(_bank2.Id).GetAllClientAccounts(pudge.Id)[0];
            _centralBank.TransferBetweenBanks(_bank.Id, _bank2.Id, 1, glebAcc.GetAccountId(), pudgeAcc.GetAccountId());
            Assert.AreEqual(0, glebAcc.GetCurrentMoney());
            Assert.AreEqual(2, pudgeAcc.GetCurrentMoney());
            
        }
    }
}
