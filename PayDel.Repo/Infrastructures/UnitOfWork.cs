using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PayDel.Repo.Repositories.Repo;
using PayDel.Repo.Repositories.Interface;
using System.Linq;
using System.Reflection;
using PayDel.Common.Helpers;
using PayDel.Repo.Repositories.FinancialDB.Interface;
using PayDel.Repo.Repositories.FinancialDB.Repo;

namespace PayDel.Repo.Infrastructures
{
    //public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        #region ctr
        protected readonly DbContext _db;
        public UnitOfWork(TContext context)
        {
            //_db = new TContext();
            _db = context;
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

        private IPhotoRepository photoRepository;
        public IPhotoRepository _PhotoRepository
        {
            get
            {
                if (photoRepository == null)
                {
                    photoRepository = new PhotoRepository(_db);
                }
                return photoRepository;
            }
        }

        private IVerificationCodeRepository verificationCodeRepository;
        public IVerificationCodeRepository _VerificationCodeRepository
        {
            get
            {
                if (verificationCodeRepository == null)
                {
                    verificationCodeRepository = new VerificationCodeRepository(_db);
                }
                return verificationCodeRepository;
            }
        }
        #region methods


        #region privateFinancialrepository
        private IEntryRepository entryRepository;
        public IEntryRepository EntryRepository
        {
            get
            {
                if (entryRepository == null)
                {
                    entryRepository = new EntryRepository(_db);
                }
                return entryRepository;
            }
        }
        private IFactorRepository factorRepository;
        public IFactorRepository FactorRepository
        {
            get
            {
                if (factorRepository == null)
                {
                    factorRepository = new FactorRepository(_db);
                }
                return factorRepository;
            }
        }
        #endregion

        public void Save()
        {
            _cleanStrings();
            _db.SaveChanges();
        }

        public async Task<int> SaveAcync()
        {
            try
            {
                _cleanStrings();
                return await _db.SaveChangesAsync();
            }
            catch (Exception)
            {

                return 0;
            }
            
        }

        private void _cleanStrings()
        {
            var changedEntities = _db.ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added || p.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.CleanString();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
            {

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