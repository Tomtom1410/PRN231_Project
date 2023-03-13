using BusinessLogic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAuthBusiness
    {
        Task <AccountDto> GetAccountAsync(string? username, string? password);
    }
}
