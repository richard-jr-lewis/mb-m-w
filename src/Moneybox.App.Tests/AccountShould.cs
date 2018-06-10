using NUnit.Framework;
using System;

namespace Moneybox.App.Tests
{
    [TestFixture]
    public class AccountShould
    {
        private Account _account;

        [SetUp]
        public void SetUp()
        {
            _account = new Account();
        }

        [TestCase(100, 0)]
        [TestCase(100, 50)]
        [TestCase(100, 70)]
        [TestCase(100, 100)]
        public void Account_CanWithdraw(decimal balance, decimal withdrawAmount)
        {
            _account.Balance = balance;

            bool canWithdraw = _account.CanWithdraw(withdrawAmount);

            Assert.That(canWithdraw, Is.True);
        }

        [TestCase(100, 101)]
        [TestCase(100, 110)]
        [TestCase(100, 200)]
        [TestCase(100, 500)]
        public void Account_CannotWithdraw(decimal balance, decimal withdrawAmount)
        {
            _account.Balance = balance;

            bool canWithdraw = _account.CanWithdraw(withdrawAmount);

            Assert.That(canWithdraw, Is.False);
        }

        [TestCase(100, 100, 10, 90, 90)]
        [TestCase(150, 0, 50, 100, -50)]
        public void Account_Withdraw(decimal initialBalance,
            decimal initialWithdrawn,
            decimal withdrawAmount,
            decimal expectedBalance,
            decimal expectedWithdrawn)
        {
            _account.Balance = initialBalance;
            _account.Withdrawn = initialWithdrawn;

            _account.Withdraw(withdrawAmount);

            Assert.That(_account.Balance, Is.EqualTo(expectedBalance));
            Assert.That(_account.Withdrawn, Is.EqualTo(expectedWithdrawn));
        }

        [TestCase(100, 101)]
        [TestCase(100, 110)]
        [TestCase(100, 200)]
        [TestCase(100, 500)]
        public void Account_InsufficientFundsThrowException(decimal balance, decimal withdrawAmount)
        {
            _account.Balance = balance;

            Assert.Throws<InvalidOperationException>(() => _account.Withdraw(withdrawAmount));
        }

        [TestCase(3900, 0)]
        [TestCase(3900, 50)]
        [TestCase(3900, 70)]
        [TestCase(3900, 100)]
        public void Account_CanPayIn(decimal paidIn, decimal payInAmount)
        {
            _account.PaidIn = paidIn;

            bool canPayIn = _account.CanPayIn(payInAmount);

            Assert.That(canPayIn, Is.True);
        }

        [TestCase(3900, 101)]
        [TestCase(3900, 110)]
        [TestCase(3900, 200)]
        [TestCase(3900, 500)]
        public void Account_CannotPayIn(decimal paidIn, decimal payInAmount)
        {
            _account.PaidIn = paidIn;

            bool canPayIn = _account.CanPayIn(payInAmount);

            Assert.That(canPayIn, Is.False);
        }

        [TestCase(100, 100, 10, 110, 110)]
        [TestCase(150, 0, 50, 200, 50)]
        public void Account_PayIn(decimal initialBalance,
            decimal initialPaidIn,
            decimal payInAmount,
            decimal expectedBalance,
            decimal expectedPaidIn)
        {
            _account.Balance = initialBalance;
            _account.PaidIn = initialPaidIn;

            _account.PayIn(payInAmount);

            Assert.That(_account.Balance, Is.EqualTo(expectedBalance));
            Assert.That(_account.PaidIn, Is.EqualTo(expectedPaidIn));
        }

        [TestCase(3900, 101)]
        [TestCase(3900, 110)]
        [TestCase(3900, 200)]
        [TestCase(3900, 500)]
        public void Account_PayInLimitReachedThrowException(decimal paidIn, decimal payInAmount)
        {
            _account.PaidIn = paidIn;

            Assert.Throws<InvalidOperationException>(() => _account.PayIn(payInAmount));
        }

        [TestCase(400)]
        [TestCase(450)]
        [TestCase(499)]
        public void Account_CanNotifyFundsLow(decimal balance)
        {
            _account.Balance = balance;

            bool canNotifyFundsLow = _account.IsLowFunds;

            Assert.That(canNotifyFundsLow, Is.True);
        }

        [TestCase(501)]
        [TestCase(550)]
        [TestCase(600)]
        public void Account_CannotNotifyFundsLow(decimal balance)
        {
            _account.Balance = balance;

            bool canNotifyFundsLow = _account.IsLowFunds;

            Assert.That(canNotifyFundsLow, Is.False);
        }

        [TestCase(3600)]
        [TestCase(3700)]
        [TestCase(3800)]
        public void Account_CanNotifyApproachingPayInLimit(decimal paidIn)
        {
            _account.PaidIn = paidIn;

            bool canNotifyFundsLow = _account.IsApproachingPayInLimit;

            Assert.That(canNotifyFundsLow, Is.True);
        }

        [TestCase(3400)]
        [TestCase(3500)]
        public void Account_CannotNotifyApproachingPayInLimit(decimal paidIn)
        {
            _account.PaidIn = paidIn;

            bool canNotifyFundsLow = _account.IsApproachingPayInLimit;

            Assert.That(canNotifyFundsLow, Is.False);
        }
    }
}
