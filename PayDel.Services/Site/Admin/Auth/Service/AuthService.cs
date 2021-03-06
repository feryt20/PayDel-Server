﻿using PayDel.Common.Helpers;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;
using PayDel.Services.Site.Admin.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Services.Site.Admin.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IUtilitiess _utilities;
        public AuthService(IUnitOfWork<PayDelDbContext> dbContext, IUtilitiess utilities)
        {
            _db = dbContext;
            _utilities = utilities;
        }

        public async Task<PayDel.Data.Models.User> Login(string username, string password)
        {
            var user = await _db._UserRepository.GetUserByUserNameAsync(username);
            if(user == null)
            {
                return null;
            }
            //if(!Utilities.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            //{
            //    return null;
            //}

            return user;
        }

        public async Task<PayDel.Data.Models.User> Register(PayDel.Data.Models.User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            _utilities.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //user.PasswordHash = passwordHash;
            //user.PasswordSalt = passwordSalt;

            await _db._UserRepository.InsertAsync(user);
            await _db.SaveAcync();

            return user;
        }
    }
}
