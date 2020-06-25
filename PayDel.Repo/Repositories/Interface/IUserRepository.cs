using PayDel.Repo.Infrastructures;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Repo.Repositories.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByUserNameAsync(string username);
        Task<bool> UserExistsAsync(string username);
    }
}
