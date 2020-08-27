using Microsoft.EntityFrameworkCore;
using PayDel.Repo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Repo.Infrastructures
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext:DbContext
    {
        IUserRepository _UserRepository { get; }
        ITokenRepository _TokenRepository { get; }
        void Save();
        Task<int> SaveAcync();
    }
}
