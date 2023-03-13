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

        public async Task<Account> getAccountAsync(string? username, string? password)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(username) && x.Password.Equals(password));
        }
    }
}
