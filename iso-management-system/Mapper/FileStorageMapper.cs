using iso_management_system.Dto.FileStorage;
using iso_management_system.Models;

namespace iso_management_system.Mappers;

public static class FileStorageMapper
{
    public static FileStorageResponseDTO ToResponseDTO(FileStorage file)
    {
        return new FileStorageResponseDTO
        {
            FileID = file.FileID,
            FileName = file.FileName,
            FilePath = file.FilePath,
            FileSize = (long)file.FileSize,
            UploadedAt = file.UploadedAt
        };
    }
}