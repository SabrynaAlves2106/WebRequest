namespace WebRequest.Application.WebRequest;

public class ApiExcelexpert : IApiExcelExpert
{
    private IFileService _fileService { get; init; }
    private ILogger<ApiExcelexpert> _logger { get; init; }
    private IConfiguration _configuration { get; init; }
    public ApiExcelexpert(IFileService fileService, ILogger<ApiExcelexpert> loger,IConfiguration configuration)
    {
        _fileService = fileService;
        _logger = loger;
        _configuration = configuration;
    }
    public async Task<IEnumerable<FileToDownload>> FindExistingFiles()
    {

        List<FileToDownload> plan = new();
        using (var httpClient = new HttpClient())
        {

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), _configuration["EndPoint:FileName"]))
            {
                var response = await httpClient.SendAsync(request);

                var findNameZipFile = await response.Content.ReadAsStringAsync();

                var htmlPage = new HtmlDocument();
                htmlPage.LoadHtml(findNameZipFile);
                var fileFolder = htmlPage.DocumentNode.SelectNodes(_configuration["Elements:NameFile"]);
                var fileType = htmlPage.DocumentNode.SelectNodes(_configuration["Elements:Pattern"]);
                var archive = fileFolder.Zip(fileType,(f,t)=> new {Folder= f, Type = t});
                
                foreach (var file in archive)
                {
                    plan.Add(new FileToDownload {
                        FileName= file.Folder.InnerText.Trim(),
                        PatternFile= file.Type.InnerText.Replace("Arquivo:", "").Trim() 
                    });
                }
            }

        }
        return plan;
    }


    public async Task DownloadZipAsync(IEnumerable<FileToDownload> namePlan)
    {
        foreach (var plan in namePlan)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), _configuration["EndPoint:DownloadFile"] +$"{plan.FileName}/{plan.PatternFile}"))
                {
                    var response = await httpClient.SendAsync(request);

                    var readZipFile = await response.Content.ReadAsStreamAsync();
                    _logger.LogInformation("Realizando o Download do arquivo {0} que pertence ao {1}",plan.PatternFile,plan.FileName);

                    using (var createFile = File.Create(_fileService.GetDirectory(plan)))
                    {
                        readZipFile.CopyTo(createFile);
                    }
                }
            }
        }
    }

}