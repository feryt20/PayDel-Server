using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Services.Seed.Interface
{
    public interface ISeedService
    {
        Task SeedUsersAsync();
        void SeedUsers();
    }
}
