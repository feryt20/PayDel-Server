using Microsoft.EntityFrameworkCore;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.DatabaseContext
{
    public class PayDelDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=PayDelDb;Integrated Security=true;MultipleActiveResultSets=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<BankCard> BankCards { get; set; }
    }
}
