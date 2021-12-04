using System;
using System.Collections.Generic;
using System.Data.Common;
using Banks.Tools;

namespace Banks.Entities
{
    public class Client
    {
        public Client(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public ulong PassportId { get; private set; } = 0;
        public string Address { get; private set; } = "none";
        public Guid Id { get; private set; } = Guid.NewGuid();
        public bool IsSubbed { get; private set; } = false;
        public List<string> NewsLogs { get; private set; } = new List<string>();
        public void UpdateSubscription(bool newState)
        {
            IsSubbed = newState;
        }

        public void UpdatePassport(ulong newPassport)
        {
            PassportId = newPassport;
        }

        public void UpdateAddress(string newAddress)
        {
            Address = newAddress;
        }

        public void AddToLog(string notification)
        {
            NewsLogs.Add(notification);
        }
    }
}