using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;
        public const decimal WithdrawLimit = 0m;
        public const decimal FundsLowNotificationLimit = 500m;
        public const decimal PayInNotificationLimit = 500m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public bool CanWithdraw(decimal withdrawAmount) => Balance - withdrawAmount >= WithdrawLimit;

        public bool CanPayIn(decimal payInAmount) => PaidIn + payInAmount <= PayInLimit;

        public bool IsLowFunds => Balance < FundsLowNotificationLimit;

        public bool IsApproachingPayInLimit => PayInLimit - PaidIn < PayInNotificationLimit;

        public void Withdraw(decimal withdrawAmount)
        {
            if (!CanWithdraw(withdrawAmount))
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            Balance -= withdrawAmount;
            Withdrawn -= withdrawAmount;
        }

        public void PayIn(decimal payInAmount)
        {
            if (!CanPayIn(payInAmount))
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            Balance += payInAmount;
            PaidIn += payInAmount;
        }
    }
}
