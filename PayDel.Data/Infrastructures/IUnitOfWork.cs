using Microsoft.EntityFrameworkCore;
using PayDel.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Data.Infrastructures
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext:DbContext
    {
        IUserRepository _UserRepository { get; }
        void Save();
        Task<int> SaveAcync();
    }
}
