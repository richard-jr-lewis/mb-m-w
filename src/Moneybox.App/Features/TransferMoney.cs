﻿using Moneybox.App.DataAccess;
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

            from.Withdraw(amount);

            to.PayIn(amount);

            _accountRepository.Update(from);
            _accountRepository.Update(to);

            if (from.IsLowFunds)
            {
                _notificationService.NotifyFundsLow(from.User.Email);
            }

            if (to.IsApproachingPayInLimit)
            {
                _notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }
        }
    }
}
