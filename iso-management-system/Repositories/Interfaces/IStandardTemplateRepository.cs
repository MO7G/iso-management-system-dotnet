using System.Collections.Generic;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IStandardTemplateRepository
    {
        void AddTemplate(StandardTemplate template);
        StandardTemplate? GetTemplateById(int templateId);
        IEnumerable<StandardTemplate> GetTemplatesBySection(int sectionId);
        IEnumerable<StandardTemplate> GetTemplatesByStandard(int standardId);
        IEnumerable<StandardTemplate> GetTemplatesBySectionId(int sectionId);
        void DeleteTemplate(StandardTemplate template); // ✅ NEW

        void SaveChanges();
    }
}