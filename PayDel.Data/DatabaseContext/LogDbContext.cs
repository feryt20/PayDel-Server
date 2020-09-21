using Microsoft.EntityFrameworkCore;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace PayDel.Data.DatabaseContext
{
    public class LogDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(@"Server=localhost , 1400;Initial Catalog=PayDel_Logdb;User Id=SA;Password=My!123456;");
            //.UseSqlServer(@"Server=(local);Initial Catalog=PayDel_Logdb;User Id=sa;Password=fery;");
        }

        public LogDbContext(DbContextOptions<LogDbContext> options)
    : base(options)
        { }

        public DbSet<ExtendedLog> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            LogModelBuilderHelper.Build(modelBuilder.Entity<ExtendedLog>());

            modelBuilder.Entity<ExtendedLog>().ToTable("ExtendedLog");
        }
    }
}
