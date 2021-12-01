namespace Banks.Entities
{
    public enum AccountType
    {
        /// <summary>
        /// DebitAccount, just a normal account
        /// </summary>
        DebitAccount,

        /// <summary>
        /// DepositAccount, earn more, spend none
        /// </summary>
        DepositAccount,

        /// <summary>
        /// CreditAccount, spend more, earn none
        /// </summary>
        CreditAccount,
    }
}