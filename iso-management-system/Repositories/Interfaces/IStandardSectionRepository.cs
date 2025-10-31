// Repositories/Interfaces/IStandardSectionRepository.cs
using System.Collections.Generic;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IStandardSectionRepository
    {
        IEnumerable<StandardSection> GetSectionsByStandard(int standardId);
        StandardSection? GetSectionById(int id);
        void AddSection(StandardSection section);
        void DeleteSection(StandardSection section);

        Task UpdateSectionAsync(StandardSection section, CancellationToken cancellationToken = default);

        bool HasChildSections(int sectionId);
        void SaveChanges();
    }
}