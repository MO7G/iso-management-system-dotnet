using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Repositories.Implementations
{
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly AppDbContext _context;

        public FileStorageRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(FileStorage file)
        {
            _context.FileStorages.Add(file);
        }

        public IEnumerable<FileStorage> GetAll()
        {
            return _context.FileStorages.ToList();
        }

        public FileStorage? GetById(int id)
        {
            return _context.FileStorages.FirstOrDefault(f => f.FileID == id);
        }

        public void Delete(FileStorage file)
        {
            _context.FileStorages.Remove(file);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}