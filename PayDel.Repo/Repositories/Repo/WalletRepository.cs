using Microsoft.EntityFrameworkCore;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Repo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Repo.Repositories.Repo
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        private readonly DbContext _db;
        public WalletRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (PayDelDbContext)dbContext;
        }

        public async Task<long> GetLastWalletCodeAsync()
        {
            return (await GetAllAsync(null, p => p.OrderByDescending(s => s.Code), "")).First().Code;
        }

        public async Task<bool> WalletCodeExistAsync(long code)
        {
            return (await GetAllAsync(p => p.Code == code, null, "")).Any();
        }

        public async Task<int> WalletCountAsync(string userId)
        {
            return (await GetAllAsync(p => p.Id == userId, null, "")).Count();
        }
    }
}
