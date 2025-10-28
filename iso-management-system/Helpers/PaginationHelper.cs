using iso_management_system.Constants;

namespace iso_management_system.Helpers;

public class PaginationHelper
{
    public static void Normalize(ref int pageNumber, ref int pageSize)
    {
        if (pageNumber < 1)
            pageNumber = PaginationDefaults.DefaultPageNumber;

        if (pageSize < 1)
            pageSize = PaginationDefaults.DefaultPageSize;

        if (pageSize > PaginationDefaults.MaxPageSize)
            pageSize = PaginationDefaults.MaxPageSize;
    }
}