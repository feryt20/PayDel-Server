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
    public class VerificationCodeRepository : Repository<VerificationCode>, IVerificationCodeRepository
    {
        private readonly DbContext _db;
        public VerificationCodeRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (PayDelDbContext)dbContext;
        }
    }
}
