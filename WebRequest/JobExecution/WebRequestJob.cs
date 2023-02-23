using Microsoft.Graph;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRequest.JobExecution;

public class WebRequestJob : JobBase
{
    private IApiExcelExpert _apiExcelExpert { get; init; }
    private IFileService _fileService { get; init; }

    public WebRequestJob(ILogger<WebRequestJob> logger, IApiExcelExpert apiExcelExpert, IFileService fileService) : base(logger)
    {
        _apiExcelExpert = apiExcelExpert;
        _fileService = fileService;
    }

    public async override Task Execute(IJobExecutionContext context)
    {
        try
        {
            _fileService.DeleteAll();
            var listNamefiles = await _apiExcelExpert.FindExistingFiles();
            await _apiExcelExpert.DownloadZipAsync(listNamefiles);
            System.Diagnostics.Process.Start("explorer.exe",$"C:\\Users\\{Environment.UserName}\\Desktop\\PocWebRequest");
            MessageBox.Show("Finalizada execução");
            Environment.Exit(0);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
