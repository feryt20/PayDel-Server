using Newtonsoft.Json;
using PayDel.Common.Helpers;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Seed.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Services.Seed.Service
{
    public class SeedService : ISeedService
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        public SeedService(IUnitOfWork<PayDelDbContext> dbContext)
        {
            _db = dbContext;
        }

        public void SeedUsers()
        {
            var count = _db._UserRepository.Count();
            if (count == 0)
            {
                var userData = System.IO.File.ReadAllText("Files/Json/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    Utilities.CreatePasswordHash("123456", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.UserName = user.UserName.ToLower();

                    _db._UserRepository.Insert(user);
                }

                _db.Save();
            }
        }
    }
}
