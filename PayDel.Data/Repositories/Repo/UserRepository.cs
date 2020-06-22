using Microsoft.EntityFrameworkCore;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Infrastructures;
using PayDel.Data.Models;
using PayDel.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.Repositories.Repo
{
    public class UserRepository : Repository<User>,IUserRepository
    {
        private readonly DbContext _db;
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            _db = _db ?? (PayDelDbContext) _db;
        }
    }
}
