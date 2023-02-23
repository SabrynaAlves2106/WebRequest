using System.Numerics;
using WebRequest.Domain.Entities;
using WebRequest.Domain.Interfaces;

namespace WebRequest.Infrastructure.Service;

public class FileService: IFileService
{
    public string GetDirectory(FileToDownload plan)
    {
        string defaultDirectory = $"C:\\Users\\{Environment.UserName}\\Desktop\\PocWebRequest\\{plan.FileName}\\";
        if (!Directory.Exists(defaultDirectory))
            Directory.CreateDirectory(defaultDirectory);
        return defaultDirectory + plan.PatternFile;
    }

    public void DeleteAll()
    {
        string defaultDirectory = $"C:\\Users\\{Environment.UserName}\\Desktop\\PocWebRequest\\";

        if (Directory.Exists(defaultDirectory))
            Directory.Delete(defaultDirectory,true);
    }

}
