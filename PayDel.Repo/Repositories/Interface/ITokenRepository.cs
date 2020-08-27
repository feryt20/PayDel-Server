using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Repo.Repositories.Interface
{
    public interface ITokenRepository : IRepository<MyToken>
    {
    }
}
