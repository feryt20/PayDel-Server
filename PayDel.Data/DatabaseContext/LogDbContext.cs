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
            optionBuilder.UseSqlServer(@"Data Source=(local);Initial Catalog=PayDel_Logdb;Integrated Security=True;MultipleActiveResultSets=True;");
        }


        public DbSet<ExtendedLog> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            LogModelBuilderHelper.Build(modelBuilder.Entity<ExtendedLog>());

            modelBuilder.Entity<ExtendedLog>().ToTable("ExtendedLog");
        }
    }
}
