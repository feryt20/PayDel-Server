using Microsoft.EntityFrameworkCore;
using PayDel.Data.DatabaseContext;
using PayDel.Repo.Infrastructures;
using PayDel.Data.Models;
using PayDel.Repo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PayDel.Common.Helpers;

namespace PayDel.Repo.Repositories.Repo
{
    public class UserRepository : Repository<User>,IUserRepository
    {
        private readonly DbContext _db;
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            _db = _db ?? (PayDelDbContext) _db;
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await GetAsync(p => p.UserName.Equals(username.ToLower()));
        }
       
        public async Task<bool> UserExistsAsync(string username)
        {
            if (await GetAsync(p => p.UserName.Equals(username.ToLower())) != null)
                return true;

            return false;
        }
    }
}
