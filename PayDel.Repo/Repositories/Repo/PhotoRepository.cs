using Microsoft.EntityFrameworkCore;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Repo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Repo.Repositories.Repo
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        private readonly DbContext _db;
        public PhotoRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (PayDelDbContext)dbContext;
        }

        

        public async Task<Photo> Get2ById(string id)
        {
            return await GetAsNoTrackingByIdAsync(p => p.Id == id);
        }
        
        public async Task<string> UpdateSaveConcurrency(Photo photo)
        {
            _db.Entry(photo).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
                return "OK";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Photo)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    return "Unable to save. " +
                        "The department was deleted by another user.";
                }

                var dbValues = (Photo)databaseEntry.ToObject();
                return setDbErrorMessage(dbValues, clientValues, _db);
            }
        }

        private string setDbErrorMessage(Photo dbValues,
                Photo clientValues, DbContext context)
        {

            if (dbValues.Alt != clientValues.Alt)
            {
                return "Alt" +
                    $"Current value: {dbValues.Alt}";
            }
            if (dbValues.Url != clientValues.Url)
            {
                return "Url" +
                    $"Current value: {dbValues.Url}";
            }
            if (dbValues.IsMain != clientValues.IsMain)
            {
                return "IsMain" +
                     $"Current value: {dbValues.IsMain}";
            }
            if (dbValues.IsDeleted != clientValues.IsDeleted)
            {
                return "IsDeleted" +
                     $"Current value: {dbValues.IsDeleted}";
            }

            return
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.";
        }
    }
}

