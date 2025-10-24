using System.Collections.Generic;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IStandardRepository
    {
        IEnumerable<Standard> GetAllStandards();
        Standard? GetStandardById(int id);
        bool StandardNameExists(string name);
        void AddStandard(Standard standard);
        void DeleteStandard(Standard standard);
        
        bool StandardExists(int standardId);

    }
}