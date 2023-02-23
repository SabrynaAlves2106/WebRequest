using WebRequest.Domain.Entities;

namespace WebRequest.Domain.Interfaces
{
    public interface IApiExcelExpert
    {
        Task<IEnumerable<FileToDownload>> FindExistingFiles();
        Task DownloadZipAsync(IEnumerable<FileToDownload> namePlan);
    }
}
