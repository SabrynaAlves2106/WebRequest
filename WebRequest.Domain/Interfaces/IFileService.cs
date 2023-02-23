using WebRequest.Domain.Entities;

namespace WebRequest.Domain.Interfaces;

public interface IFileService
{
    string GetDirectory(FileToDownload plan);
    void DeleteAll();
}
