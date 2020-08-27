using Microsoft.EntityFrameworkCore;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Repo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Repo.Repositories.Repo
{
    public class TokenRepository : Repository<MyToken>, ITokenRepository
    {
        private readonly DbContext _db;
        public TokenRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (PayDelDbContext)_db;
        }
    }
}
