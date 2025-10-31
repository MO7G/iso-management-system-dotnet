using System.Collections.Generic;
using iso_management_system.Dto.General;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IStandardRepository
    {
        public IEnumerable<Standard> GetAllStandards(int pageNumber, int pageSize, out int totalRecords);

        Standard? GetStandardById(int id);

        public (bool Exists, bool HasSections, bool HasProjects) GetStandardDeletionStatus(int id);
        bool IsStandardUsedInAnyProject(int standardId);
        Task UpdateStandardAsync(Standard standard, CancellationToken cancellationToken = default);

        bool StandardNameExists(string name);

        public IEnumerable<Standard> SearchStandards(
            string? query,
            int pageNumber,
            int pageSize,
            SortingParameters sorting,
            out int totalRecords);
        void AddStandard(Standard standard);

        public void DeleteStandardById(int id);

        
        bool StandardExists(int standardId);

    }
}