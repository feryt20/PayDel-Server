using Microsoft.EntityFrameworkCore;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Repo.Repositories.FinancialDB.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Repo.Repositories.FinancialDB.Repo
{
    public class FactorRepository : Repository<Factor>, IFactorRepository
    {
        private readonly DbContext _db;
        public FactorRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (FinDbContext)dbContext;
        }
    }
}
