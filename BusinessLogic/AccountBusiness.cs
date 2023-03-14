using BusinessLogic.Interfaces;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IAccountRepositoy _accountRepositoy;

        public AccountBusiness(IAccountRepositoy accountRepositoy)
        {
            _accountRepositoy = accountRepositoy;
        }

        public async Task<Account> GetAccountById(long accountId)
        {
            return await _accountRepositoy.GetAccountById(accountId);
        }
    }
}
