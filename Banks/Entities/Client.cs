using System.Collections.Generic;

namespace Banks.Entities
{
    public class Client
    {
        public Client(string name, string surname)
        {
            Name = name;
            Surname = surname;
            PassportId = 0;
            Address = "none";
            IsSubbed = false;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public int PassportId { get; private set; }
        public string Address { get; private set; }
        public bool IsSubbed { get; private set; }
        public List<string> NewsLogs { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        public void ChangeSubscriptionState(bool newState)
        {
            IsSubbed = newState;
        }// trash
    }
}