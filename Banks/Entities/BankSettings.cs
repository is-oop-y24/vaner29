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

        public BankSettings()
        {
        }

        public string Name { get; private set; } = "Sber";
        public bool PassportNeeded { get; private set; } = true;
        public bool AddressNeeded { get; private set; } = true;
        public decimal SusLimit { get; private set; } = 100;
        public int DebitRemainderPercentage { get; private set; } = 36500;
        public decimal CreditLimit { get; private set; } = 200;
        public int CreditCommissionPercentage { get; private set; } = 36500;
        public int DepositTerm { get; private set; } = 361;

        public List<DepositPercentageRange> DepositPercentageRanges { get; private set; } =
            new List<DepositPercentageRange>
                { new DepositPercentageRange(0, 500, 365), new DepositPercentageRange(501, decimal.MaxValue, 36500) };

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