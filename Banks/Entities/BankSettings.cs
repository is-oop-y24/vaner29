using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class BankSettings
    {
        public BankSettings(
            string name,
            bool passportNeeded,
            bool addressNeeded,
            decimal susLimit,
            int debitRemainderPercentage,
            decimal creditLimit,
            int creditCommissionPercentage,
            int depositTerm,
            List<DepositPercentageRange> depositPercentageRanges)
        {
            Name = name;
            PassportNeeded = passportNeeded;
            AddressNeeded = addressNeeded;
            if (susLimit < 0)
                throw new BankException("Limit for sus people can't be < 0");
            SusLimit = susLimit;
            if (debitRemainderPercentage < 0)
                throw new BankException("Debit percentage can't be < 0");
            DebitRemainderPercentage = debitRemainderPercentage;
            if (creditLimit <= 0)
                throw new BankException("Credit limit can't be <= 0, That's not a credit account anymore");
            CreditLimit = creditLimit;
            if (creditCommissionPercentage < 0)
                throw new BankException("Credit commission can't be < 0");
            CreditCommissionPercentage = creditCommissionPercentage;
            if (depositTerm < 0)
                throw new BankException("Deposit Term can't be < 0");
            DepositTerm = depositTerm;
            if (!(depositPercentageRanges.Exists(x => x.Min == 0) &&
                  depositPercentageRanges.Exists(x => x.Max == decimal.MaxValue)))
                throw new BankException("The ranges have to cover everything from 0 to maxvalue");
            DepositPercentageRanges = depositPercentageRanges;
        }

        public string Name { get; private set; }
        public bool PassportNeeded { get; private set; }
        public bool AddressNeeded { get; private set; }
        public decimal SusLimit { get; private set; }
        public int DebitRemainderPercentage { get; private set; }
        public decimal CreditLimit { get; private set; }
        public int CreditCommissionPercentage { get; private set; }
        public int DepositTerm { get; private set; }

        public List<DepositPercentageRange> DepositPercentageRanges { get; private set; }

        public void UpdateName(string newName)
        {
            Name = newName;
        }

        public void UpdatePassportNeed(bool newNeed)
        {
            PassportNeeded = newNeed;
        }

        public void UpdateAddressNeed(bool newNeed)
        {
            AddressNeeded = newNeed;
        }

        public void UpdateSusLimit(decimal newLimit)
        {
            SusLimit = newLimit;
        }

        public void UpdateDebitPercentage(int newPercentage)
        {
            DebitRemainderPercentage = newPercentage;
        }

        public void UpdateCreditLimit(decimal newLimit)
        {
            if (newLimit <= 0)
                throw new BankException("Credit limit can't be <= 0, That's not a credit account anymore");
            CreditLimit = newLimit;
        }

        public void UpdateCreditPercentage(int newPercentage)
        {
            CreditCommissionPercentage = newPercentage;
        }

        public void UpdateDepositTerm(int newTerm)
        {
            DepositTerm = newTerm;
        }

        public void UpdateDepositRanges(List<DepositPercentageRange> newRanges)
        {
            if (!(newRanges.Exists(x => x.Min == 0) &&
                  newRanges.Exists(x => x.Max == decimal.MaxValue)))
                throw new BankException("The ranges have to cover everything from 0 to maxvalue");
            DepositPercentageRanges = newRanges;
        }
    }
}