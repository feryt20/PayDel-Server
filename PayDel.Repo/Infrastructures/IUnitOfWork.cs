using Microsoft.EntityFrameworkCore;
using PayDel.Repo.Repositories.FinancialDB.Interface;
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
        IGateRepository _GateRepository { get; }
        IWalletRepository _WalletRepository { get; }

        IPhotoRepository _PhotoRepository { get; }

        IVerificationCodeRepository _VerificationCodeRepository { get; }

        IEntryRepository EntryRepository { get; }
        IFactorRepository FactorRepository { get; }

        void Save();
        Task<int> SaveAcync();
    }
}
