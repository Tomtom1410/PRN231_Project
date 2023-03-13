using BusinessLogic.Dtos;
using BusinessLogic.Interfaces;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly IAccountRepositoy _accountRepository;
        public AuthBusiness(IAccountRepositoy accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<AccountDto> GetAccountAsync(string? username, string? password)
        {
            var account = await _accountRepository.getAccountAsync(username, password) ;
            if (account == null)
            {
                return null;
            }

            return new AccountDto()
            {
                Id = account.Id,
                FullName = account.FullName,
                Username = account.Username,
                IsTeacher = account.IsTeacher,
            };
        }
    }
}
