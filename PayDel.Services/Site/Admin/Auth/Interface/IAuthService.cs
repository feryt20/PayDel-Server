using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Services.Site.Admin.Auth.Interface
{
    public interface IAuthService
    {
        Task<PayDel.Data.Models.User> Register(PayDel.Data.Models.User user, string password);
        Task<PayDel.Data.Models.User> Login(string username, string password);
    }
}
