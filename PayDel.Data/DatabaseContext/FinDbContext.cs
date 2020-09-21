using Microsoft.EntityFrameworkCore;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.DatabaseContext
{
    public class FinDbContext : DbContext
    {
        public FinDbContext()
        {
        }
        public FinDbContext(DbContextOptions<FinDbContext> opt) : base(opt)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(@"Server=localhost , 1400;Initial Catalog=PayDel_FinDb;User Id=SA;Password=My!123456;");
            //.UseSqlServer(@"Server=(local);Initial Catalog=PayDel_FinDb;User Id=sa;Password=fery;");
        }

        public DbSet<Factor> Factors { get; set; }
        public DbSet<Entry> Entries { get; set; }
    }
}
