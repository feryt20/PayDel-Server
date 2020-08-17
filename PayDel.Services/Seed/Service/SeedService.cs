using Microsoft.AspNetCore.Identity;
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
        //private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        //public SeedService(IUnitOfWork<PayDelDbContext> dbContext)
        public SeedService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //_db = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //public void SeedUsers()
        //{
        //    var count = _db._UserRepository.Count();
        //    if (count == 0)
        //    {
        //        var userData = System.IO.File.ReadAllText("Files/Json/UserSeedData.json");
        //        var users = JsonConvert.DeserializeObject<List<User>>(userData);
        //        foreach (var user in users)
        //        {
        //            byte[] passwordHash, passwordSalt;
        //            Utilities.CreatePasswordHash("123456", out passwordHash, out passwordSalt);

        //            //user.PasswordHash = passwordHash;
        //            //user.PasswordSalt = passwordSalt;
        //            user.UserName = user.UserName.ToLower();

        //            _db._UserRepository.Insert(user);
        //        }

        //        _db.Save();
        //    }
        //}

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Files/Json/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<IList<User>>(userData);

                var roles = new List<Role>
                {
                    new Role {Name = "Admin"},
                    new Role {Name = "User"},
                    new Role {Name = "Vip"},
                    new Role {Name = "Operator"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    user.Email = user.Email.ToLower();
                    user.UserName = user.UserName.ToLower();
                    _userManager.CreateAsync(user, "MyP@ssword!").Wait();
                    //_userManager.AddToRoleAsync(user, "Admin").Wait();
                    _userManager.AddToRolesAsync(user, new[] { "Admin" , "User" , "Vip" , "Operator" }).Wait();
                }
            }
        }

    }
}
