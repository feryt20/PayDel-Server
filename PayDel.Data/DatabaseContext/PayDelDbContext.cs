using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PayDel.Data.DatabaseContext
{
    public class PayDelDbContext : IdentityDbContext<User,Role, string,
        IdentityUserClaim<string>, UserRole,IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public PayDelDbContext()
        {

        }
        public PayDelDbContext(DbContextOptions<PayDelDbContext> opt ) : base(opt)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=PayDelDb;Integrated Security=true;MultipleActiveResultSets=True;");

        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<BankCard> BankCards { get; set; }
        public DbSet<MyToken> MyTokens { get; set; }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Gate> Gates { get; set; }
        public DbSet<EasyPay> EasyPays { get; set; }
        //public DbSet<VerificationCode> VerificationCodes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Photo>()
               .Property(x => x.RowVersion)
               .IsConcurrencyToken()
               .ValueGeneratedOnAddOrUpdate();
        }

    }
}
