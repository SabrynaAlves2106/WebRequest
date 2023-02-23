

namespace WebRequest.Application.Configuration;

public static class AppDependencyInjection
{
    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        //! Alterar para repositorio verdadeiro
        services.AddTransient<IApiExcelExpert, ApiExcelexpert>();
        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}
