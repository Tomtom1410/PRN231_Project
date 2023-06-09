﻿using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAccountRepositoy
    {
        Task<Account> GetAccountAsync(string? username, string? password);
        Task<Account> GetAccountById(long accountId);
    }
}
