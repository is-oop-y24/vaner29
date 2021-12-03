using System.Collections.Generic;
using Banks.Entities;

namespace Banks.Services
{
    public class CentralBank
    {
        private IReadOnlyList<Bank> _banks;
    }
}