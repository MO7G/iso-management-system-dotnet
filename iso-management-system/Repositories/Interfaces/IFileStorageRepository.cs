using System.Collections.Generic;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IFileStorageRepository
    {
        void Add(FileStorage file);
        IEnumerable<FileStorage> GetAll();
        FileStorage? GetById(int id);
        void Delete(FileStorage file);
        void SaveChanges();
    }
}