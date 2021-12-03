using System;
using System.Collections.Generic;
using Banks.Interfaces;

namespace Banks.Entities
{
    public interface ITransaction
    {
        void CancelTransaction();
        List<IAccount> GetClientAccounts();
    }
}