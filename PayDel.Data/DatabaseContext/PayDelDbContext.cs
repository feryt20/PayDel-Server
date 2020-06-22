using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Data.DatabaseContext
{
    class PayDelDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("DataSource=.;Initial Catalog=PayDelDb;Integrated Security=true;MultipleActiveResultSets=True;");

        }
    }
}
