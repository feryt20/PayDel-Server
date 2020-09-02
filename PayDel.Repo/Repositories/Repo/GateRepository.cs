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
    public class GateRepository : Repository<Gate>, IGateRepository
    {
        private readonly DbContext _db;
        public GateRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (PayDelDbContext)dbContext;
        }
    }
}
