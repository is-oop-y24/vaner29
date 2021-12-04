using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities;

namespace Banks
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var centralBank = new CentralBank();
            bool alive = true;
            while (alive)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("1. Создать банк  \t 2. Создать клиента");
                Console.WriteLine("3. Создать счёт \t 4. Произвести/Отменить транзакцию \t 5. Пропустить дни");
                Console.WriteLine("6. Изменить настройки банка \t 7. Проверить состояние клиента \t 8. Проверить состояние банка");
                Console.WriteLine("9. Выйти из программы");
                Console.WriteLine("Введите номер пункта:");
                Console.ForegroundColor = color;
                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            CreateBank(centralBank);
                            break;
                        case 2:
                            CreateClient(centralBank);
                            break;
                        case 3:
                            CreateAccount(centralBank);
                            break;
                        case 4:
                            MakeTransaction(centralBank);
                            break;
                        case 5:
                            SkipDays(centralBank);
                            break;
                        case 6:
                            UpdateBank(centralBank);
                            break;
                        case 7:
                            CheckClient(centralBank);
                            break;
                        case 8:
                            CheckBank(centralBank);
                            break;
                        case 9:
                            alive = false;
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
        }

        private static void CreateBank(CentralBank centralBank)
        {
            Console.WriteLine("Укажите название банка:");
            string name = Console.ReadLine();
            Console.WriteLine("Укажите, нужен ли Паспорт для получения \"Неподозрительного\" статуса (true/false):");
            bool passport = Convert.ToBoolean(Console.ReadLine());
            Console.WriteLine("Укажите, нужен ли Адрес для получения \"Неподозрительного\" статуса (true/false):");
            bool address = Convert.ToBoolean(Console.ReadLine());
            Console.WriteLine("Укажите максимальный объем денег которым могут распоряжаться подозрительные аккаунты");
            decimal susLimit = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Укажите процентную ставку для ДЕБЕТОВЫХ счетов");
            int debitPercentage = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Укажите кредитный лимит для КРЕДИТНЫХ счетов");
            decimal creditLimit = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Укажите комиссию для КРЕДИТНЫХ счетов");
            int creditPercentage = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Укажите срок для ДЕПОЗИТНЫХ счетов");
            int depositTerm = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Укажите сколько будет \"отрезков\" для ставки ДЕПОЗИТНЫХ счетов");
            decimal n = Convert.ToDecimal(Console.ReadLine());
            var depositRanges = new List<DepositPercentageRange>();
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Укажите минимальное значение отрезка");
                decimal min = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Укажите максимальное значение отрезка, -1 для максимально возможного");
                decimal max = Convert.ToDecimal(Console.ReadLine());
                if (max == -1)
                    max = decimal.MaxValue;
                Console.WriteLine("Укажите процентную ставку");
                int percentage = Convert.ToInt32(Console.ReadLine());
                depositRanges.Add(new DepositPercentageRange(min, max, percentage));
            }

            centralBank.AddBank(new BankSettings(name, passport, address, susLimit, debitPercentage, creditLimit, creditPercentage, depositTerm, depositRanges));
            Console.WriteLine("id банка: " + centralBank.Banks.Last().Id);
        }

        private static void CreateClient(CentralBank centralBank)
        {
            Console.WriteLine("Выберите, в каком банке создать клиента?");
            for (int i = 0; i < centralBank.Banks.Count; i++)
            {
                Console.WriteLine(i + ". " + centralBank.Banks[i].Settings.Name);
            }

            int bankNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Укажите имя клиента");
            string name = Console.ReadLine();
            Console.WriteLine("Укажите фамилию клиента");
            string surname = Console.ReadLine();
            Console.WriteLine("Укажите номер пасспорта, 0 чтобы пропустить");
            ulong passport = Convert.ToUInt64(Console.ReadLine());
            Console.WriteLine("Укажите адрес клиента, none чтобы пропустить");
            string address = Console.ReadLine();
            var client = new Client(name, surname);
            if (passport != 0)
                client.UpdatePassport(passport);
            if (address != "none")
                client.UpdateAddress(address);
            centralBank.Banks[bankNumber].AddClient(client);
            Console.WriteLine("id клиента: " + client.Id);
        }

        private static void CreateAccount(CentralBank centralBank)
        {
            Console.WriteLine("Выберите, в каком банке создать счёт?");
            for (int i = 0; i < centralBank.Banks.Count; i++)
            {
                Console.WriteLine(i + ". " + centralBank.Banks[i].Settings.Name);
            }

            int bankNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите Id клиента для создания счёта");
            Guid clientId = Guid.Parse(Console.ReadLine());
            Console.WriteLine("Введите кол-во денег чтобы сразу положить на счёт");
            decimal money = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Выберите тип счёта:");
            Console.WriteLine("1. Дебетовый счёт \t 2. Депозитный счёт  \t 3. Кредитный счёт");
            int command = Convert.ToInt32(Console.ReadLine());
            switch (command)
            {
                case 1:
                    centralBank.Banks[bankNumber].AddDebitAccount(clientId, money);
                    Console.WriteLine("id счёта: " + centralBank.Banks[bankNumber].Accounts.Last().GetAccountId());
                    break;
                case 2:
                    centralBank.Banks[bankNumber].AddDepositAccount(clientId, money);
                    Console.WriteLine("id счёта: " + centralBank.Banks[bankNumber].Accounts.Last().GetAccountId());
                    break;
                case 3:
                    centralBank.Banks[bankNumber].AddCreditAccount(clientId, money);
                    Console.WriteLine("id счёта: " + centralBank.Banks[bankNumber].Accounts.Last().GetAccountId());
                    break;
            }
        }

        private static void MakeTransaction(CentralBank centralBank)
        {
            Console.WriteLine("Выберите, в каком банке происходит операция");
            for (int i = 0; i < centralBank.Banks.Count; i++)
            {
                Console.WriteLine(i + ". " + centralBank.Banks[i].Settings.Name);
            }

            Console.WriteLine(centralBank.Banks.Count + ". Операция - перевод между банками");
            Console.WriteLine(centralBank.Banks.Count + 1 + ". Отменить операцию");
            int bankNumber = Convert.ToInt32(Console.ReadLine());
            if (bankNumber < centralBank.Banks.Count)
            {
                Console.WriteLine("Введите Id счёта инициализирующего транзакцию");
                Guid inAccId = Guid.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество денег которые вы хотите использовать");
                decimal money = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Выберите тип транзакции:");
                Console.WriteLine("1. Положить деньги \t 2. Снять деньги \t 3. Перевести деньги");
                int command = Convert.ToInt32(Console.ReadLine());
                switch (command)
                {
                    case 1:
                        centralBank.Banks[bankNumber].Put(inAccId, money);
                        Console.WriteLine("id транзакции: " + centralBank.Banks[bankNumber].Transactions.Last().GetId());
                        break;
                    case 2:
                        centralBank.Banks[bankNumber].Withdraw(inAccId, money);
                        Console.WriteLine("id транзакции: " + centralBank.Banks[bankNumber].Transactions.Last().GetId());
                        break;
                    case 3:
                        Console.WriteLine("Введите Id счёта принимающего транзакцию");
                        Guid targetId = Guid.Parse(Console.ReadLine());
                        centralBank.Banks[bankNumber].Transfer(inAccId, targetId, money);
                        Console.WriteLine("id транзакции: " + centralBank.Banks[bankNumber].Transactions.Last().GetId());
                        break;
                }
            }

            if (bankNumber == centralBank.Banks.Count)
            {
                Console.WriteLine("Введите Id банка инициализирующего транзакцию");
                Guid outBankId = Guid.Parse(Console.ReadLine());
                Console.WriteLine("Введите Id банка принимающего транзакцию");
                Guid inBankId = Guid.Parse(Console.ReadLine());
                Console.WriteLine("Введите Id счёта инициализирующего транзакцию");
                Guid outAccId = Guid.Parse(Console.ReadLine());
                Console.WriteLine("Введите Id счёта принимающего транзакцию");
                Guid inAccId = Guid.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество денег которые вы хотите использовать");
                decimal money = Convert.ToDecimal(Console.ReadLine());
                centralBank.TransferBetweenBanks(outBankId, inBankId, money, outAccId, inAccId);
                Console.WriteLine("id транзакции: " + centralBank.GetBankById(outBankId).Accounts.Last().GetAccountId());
            }

            if (bankNumber == centralBank.Banks.Count + 1)
            {
                Console.WriteLine("Введите Id банка инициализирующего транзакцию");
                Guid bankId = Guid.Parse(Console.ReadLine());
                Console.WriteLine("Введите Id транзакции которую нужно отменить");
                Guid transactionId = Guid.Parse(Console.ReadLine());
                centralBank.GetBankById(bankId).CancelTransaction(transactionId);
            }
        }

        private static void SkipDays(CentralBank centralBank)
        {
            Console.WriteLine("Введите количество дней которое вы хотите пропустить");
            int days = Convert.ToInt32(Console.ReadLine());
            centralBank.IncrementDays(days);
        }

        private static void UpdateBank(CentralBank centralBank)
        {
            Console.WriteLine("Введите Id банка");
            Guid bankId = Guid.Parse(Console.ReadLine());
            Console.WriteLine("Выберите опцию:");
            Console.WriteLine("1. Изменить Имя \t 2. Изменить требование паспорта  \t 3. Изменить требование адреса");
            Console.WriteLine("4. Изменить лимит подозрительных аккаунтов \t 5. Изменить процентную ставку Дебетовых счетов ");
            Console.WriteLine("6. Изменить лимит кредитных счетов \t 7. Изменить Комиссию кредитных счетов");
            Console.WriteLine("8. Изменить срок депозитных счетов \t 9. Изменить процентные ставки депозитных счетов");
            int command = Convert.ToInt32(Console.ReadLine());
            switch (command)
            {
                case 1:
                    Console.WriteLine("Укажите название банка:");
                    string name = Console.ReadLine();
                    centralBank.GetBankById(bankId).UpdateName(name);
                    break;
                case 2:
                    Console.WriteLine("Укажите, нужен ли Паспорт для получения \"Неподозрительного\" статуса (true/false):");
                    bool passport = Convert.ToBoolean(Console.ReadLine());
                    centralBank.GetBankById(bankId).UpdatePassportNeed(passport);
                    break;
                case 3:
                    Console.WriteLine("Укажите, нужен ли Адрес для получения \"Неподозрительного\" статуса (true/false):");
                    bool address = Convert.ToBoolean(Console.ReadLine());
                    centralBank.GetBankById(bankId).UpdateAddressNeed(address);
                    break;
                case 4:
                    Console.WriteLine("Укажите максимальный объем денег которым могут распоряжаться подозрительные аккаунты");
                    decimal susLimit = Convert.ToDecimal(Console.ReadLine());
                    centralBank.GetBankById(bankId).UpdateSusLimit(susLimit);
                    break;
                case 5:
                    Console.WriteLine("Укажите процентную ставку для ДЕБЕТОВЫХ счетов");
                    int debitPercentage = Convert.ToInt32(Console.ReadLine());
                    centralBank.GetBankById(bankId).UpdateDebitPercentage(debitPercentage);
                    break;
                case 6:
                    Console.WriteLine("Укажите кредитный лимит для КРЕДИТНЫХ счетов");
                    decimal creditLimit = Convert.ToDecimal(Console.ReadLine());
                    centralBank.GetBankById(bankId).UpdateCreditLimit(creditLimit);
                    break;
                case 7:
                    Console.WriteLine("Укажите комиссию для КРЕДИТНЫХ счетов");
                    int creditPercentage = Convert.ToInt32(Console.ReadLine());
                    centralBank.GetBankById(bankId).UpdateCreditPercentage(creditPercentage);
                    break;
                case 8:
                    Console.WriteLine("Укажите срок для ДЕПОЗИТНЫХ счетов");
                    int depositTerm = Convert.ToInt32(Console.ReadLine());
                    centralBank.GetBankById(bankId).UpdateDepositTerm(depositTerm);
                    break;
                case 9:
                    Console.WriteLine("Укажите сколько будет \"отрезков\" для ставки ДЕПОЗИТНЫХ счетов");
                    decimal n = Convert.ToDecimal(Console.ReadLine());
                    var depositRanges = new List<DepositPercentageRange>();
                    for (int i = 0; i < n; i++)
                    {
                        Console.WriteLine("Укажите минимальное значение отрезка");
                        decimal min = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Укажите максимальное значение отрезка, -1 для максимально возможного");
                        decimal max = Convert.ToDecimal(Console.ReadLine());
                        if (max == -1)
                            max = decimal.MaxValue;
                        Console.WriteLine("Укажите процентную ставку");
                        int percentage = Convert.ToInt32(Console.ReadLine());
                        depositRanges.Add(new DepositPercentageRange(min, max, percentage));
                    }

                    centralBank.GetBankById(bankId).UpdateDepositRanges(depositRanges);
                    break;
            }
        }

        private static void CheckClient(CentralBank centralBank)
        {
            Console.WriteLine("Выберите, в каком банке находятся счета клиента");
            for (int i = 0; i < centralBank.Banks.Count; i++)
            {
                Console.WriteLine(i + ". " + centralBank.Banks[i].Settings.Name);
            }

            int bankNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите Id клиента");
            Guid clientId = Guid.Parse(Console.ReadLine());
            Console.WriteLine("Имя клиента: " + centralBank.Banks[bankNumber].GetClientById(clientId).Name);
            Console.WriteLine("Фамилия клиента: " + centralBank.Banks[bankNumber].GetClientById(clientId).Surname);
            Console.WriteLine("Пасспорт клиента: " + centralBank.Banks[bankNumber].GetClientById(clientId).PassportId);
            Console.WriteLine("Адрес клиента: " + centralBank.Banks[bankNumber].GetClientById(clientId).Address);
            Console.WriteLine("Счета клиента:");
            foreach (var account in centralBank.Banks[bankNumber].GetAllClientAccounts(clientId))
            {
                Console.WriteLine("Id счёта: " + account.GetAccountId());
                Console.WriteLine("Тип счёта: " + account.GetAccountType());
                Console.WriteLine("Деньги на аккаунте: " + account.GetCurrentMoney());
                Console.WriteLine("Возраст аккаунта: " + account.GetAccountAge());
            }
        }

        private static void CheckBank(CentralBank centralBank)
        {
            Console.WriteLine("Выберите, состояние какого банка проверить");
            for (int i = 0; i < centralBank.Banks.Count; i++)
            {
                Console.WriteLine(i + ". " + centralBank.Banks[i].Settings.Name);
            }

            int bankNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Id банка: " + centralBank.Banks[bankNumber].Id);
            Console.WriteLine("Имя банка: " + centralBank.Banks[bankNumber].Settings.Name);
            Console.WriteLine("Требование паспорта: " + centralBank.Banks[bankNumber].Settings.PassportNeeded);
            Console.WriteLine("Требование адреса: " + centralBank.Banks[bankNumber].Settings.AddressNeeded);
            Console.WriteLine("Лимит для подозрительных счетов: " + centralBank.Banks[bankNumber].Settings.SusLimit);
            Console.WriteLine("Процентная ставка дебетовых счетов: " + centralBank.Banks[bankNumber].Settings.DebitRemainderPercentage);
            Console.WriteLine("Кредитный лимит: " + centralBank.Banks[bankNumber].Settings.CreditLimit);
            Console.WriteLine("Комиссия кредитных счетов: " + centralBank.Banks[bankNumber].Settings.CreditCommissionPercentage);
            Console.WriteLine("Срок депозитных аккаунтов: " + centralBank.Banks[bankNumber].Settings.DepositTerm);
            Console.WriteLine("Проценты для депозитных аккаунтов:");
            foreach (var range in centralBank.Banks[bankNumber].Settings.DepositPercentageRanges)
            {
                Console.WriteLine("От " + range.Min + " до " + range.Max + " ставка равна " + range.Percentage);
            }
        }
    }
}
