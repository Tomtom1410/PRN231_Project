using DataAccess.Dtos;

namespace BusinessLogic.Interfaces
{
    public interface IAuthBusiness
    {
        Task <AccountDto> GetAccountAsync(string? username, string? password);
    }
}
