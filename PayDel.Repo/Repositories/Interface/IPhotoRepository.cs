using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayDel.Repo.Repositories.Interface
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<Photo> Get2ById(string id);
        Task<string> UpdateSaveConcurrency(Photo photo);
    }
   
}
