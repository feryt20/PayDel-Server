using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Repo.Repositories.Interface
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<int> WalletCountAsync(string userId);
        Task<bool> WalletCodeExistAsync(long code);
        Task<long> GetLastWalletCodeAsync();
    }
}
