using iso_management_system.Constants;

namespace iso_management_system.Dto.General;

public class PaginationParameters
{
    public int PageNumber { get; set; } = PaginationDefaults.DefaultPageNumber;
    public int PageSize { get; set; } = PaginationDefaults.DefaultPageSize;
}
