using Banks.Tools;

namespace Banks.Entities
{
    public class DepositPercentageRange
    {
        public DepositPercentageRange(decimal min, decimal max, int percentage)
        {
            if (min > max)
                throw new BankException("min can't be more than max");
            Min = min;
            Max = max;
            Percentage = percentage;
        }

        public decimal Min { get; private set; }
        public decimal Max { get; private set; }
        public int Percentage { get; private set; }
    }
}