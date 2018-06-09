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

        [TestCase(100, 101)]
        [TestCase(100, 110)]
        [TestCase(100, 200)]
        [TestCase(100, 500)]
        public void Account_InsufficientFundsThrowException(decimal balance, decimal withdrawAmount)
        {
            _account.Balance = balance;

            Assert.Throws<InvalidOperationException>(() => _account.CheckWithdraw(withdrawAmount));
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

        [TestCase(3900, 101)]
        [TestCase(3900, 110)]
        [TestCase(3900, 200)]
        [TestCase(3900, 500)]
        public void Account_PayInLimitReachedThrowException(decimal paidIn, decimal payInAmount)
        {
            _account.PaidIn = paidIn;

            Assert.Throws<InvalidOperationException>(() => _account.CheckPayIn(payInAmount));
        }
    }
}
