using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories
{
    public class AccountRepository : IAccountRepositoy
    {
        private readonly Prn231ProjectContext _dbContext;

        public AccountRepository(Prn231ProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> GetAccountAsync(string? username, string? password)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(username) && x.Password.Equals(password));
        }

        public async Task<Account> GetAccountById(long accountId)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);
        }
    }
}
