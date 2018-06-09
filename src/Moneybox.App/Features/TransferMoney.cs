using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository _accountRepository;
        private INotificationService _notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            _accountRepository = accountRepository;
            _notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = _accountRepository.GetAccountById(fromAccountId);
            var to = _accountRepository.GetAccountById(toAccountId);

            var fromBalance = from.Balance - amount;
            from.CheckWithdraw(fromBalance);

            if (fromBalance < Account.FundsLowNotificationLimit)
            {
                _notificationService.NotifyFundsLow(from.User.Email);
            }

            var paidIn = to.PaidIn + amount;
            to.CheckPayIn(paidIn);

            if (Account.PayInLimit - paidIn < Account.PayInNotificationLimit)
            {
                _notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }

            from.Balance = fromBalance;
            from.Withdrawn = from.Withdrawn - amount;

            to.Balance = to.Balance + amount;
            to.PaidIn = paidIn;

            _accountRepository.Update(from);
            _accountRepository.Update(to);
        }
    }
}
