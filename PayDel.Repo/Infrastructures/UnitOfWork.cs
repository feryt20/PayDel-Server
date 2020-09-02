using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PayDel.Repo.Repositories.Repo;
using PayDel.Repo.Repositories.Interface;

namespace PayDel.Repo.Infrastructures
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        #region ctr
        protected readonly DbContext _db;
        public UnitOfWork()
        {
            _db = new TContext();
        }
        #endregion


        private IUserRepository userRepository;
        public IUserRepository _UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(_db);
                }
                return userRepository;
            }
        }


        private ITokenRepository tokenRepository;
        public ITokenRepository _TokenRepository
        {
            get
            {
                if (tokenRepository == null)
                {
                    tokenRepository = new TokenRepository(_db);
                }
                return tokenRepository;
            }
        }

        private IGateRepository gateRepository;
        public IGateRepository _GateRepository
        {
            get
            {
                if (gateRepository == null)
                {
                    gateRepository = new GateRepository(_db);
                }
                return gateRepository;
            }
        }

        private IWalletRepository walletRepository;
        public IWalletRepository _WalletRepository
        {
            get
            {
                if (walletRepository == null)
                {
                    walletRepository = new WalletRepository(_db);
                }
                return walletRepository;
            }
        }
        #region methods
        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<int> SaveAcync()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch (Exception)
            {

                return 0;
            }
            
        }
        #endregion

        #region dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion dispose
    }
}