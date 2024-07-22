using System.Net;
using HE.DocumentService.Api.Configuration;
using HE.DocumentService.Api.Extensions;
using HE.DocumentService.SharePoint.Configuration;
using HE.DocumentService.SharePoint.Extensions;
using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.TestApp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost CreateApplicationHost()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Program>()
                    .Build();

            var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        // Adding the DI container for configuration
                        services.Configure<AppConfig>(configuration.GetSection("AppConfiguration"));
                        services.AddConfigs();
                        services.AddSharePointServices();
                        services.AddAutoMapper(typeof(SpAutoMapperProfile));
                    })
                    .Build();

            return host;
        }

        using var host = CreateApplicationHost();
        var spService = host.Services.GetRequiredService<ISharePointFilesService>();

        if (Consts.SPListTitle == "Some Test SharePoint Library")
        {
            throw new NotImplementedException("Create a new SharePoint list or use an existing one, then update the Consts class to test DocumentService methods!");
        }

        var filePath = "TestFile1.txt";

        //CreateFolders(spService);

        //await UploadFile(spService, filePath);

        await GetFiles(spService);

    }

    private static void CreateFolders(ISharePointFilesService spService)
    {
        for (var i = 1; i < 1000; i++)
        {
            var rootFolder = i.ToString().PadLeft(4, '0');
            var folderPaths = new List<string>()
            {
                $"{rootFolder}/internal/a",
                $"{rootFolder}/internal/b",
                $"{rootFolder}/external/a",
                $"{rootFolder}/external/b",
            };
            spService.CreateFolders(Consts.SPListTitle, folderPaths);
        }
    }

    private static async Task UploadFile(ISharePointFilesService spService, string filePath)
    {
        using Stream stream = File.OpenRead(filePath);
        IFormFile file = new FormFile(stream, stream.Position, stream.Length, "", filePath);

        var fileUpload = new FileUploadModel()
        {
            File = file,
            FolderPath = "0002/internal/b",
            ListTitle = Consts.SPListTitle,
            PartitionId = "0002",
        };
        await spService.UploadFile(fileUpload);
    }

    private static async Task GetFiles(ISharePointFilesService spService)
    {
        var filter = new FileTableFilter()
        {
            ListTitle = Consts.SPListTitle,
            ListAlias = Consts.SPListAlias,
            FolderPaths = new List<string>() { "0002/internal/b" },
            PartitionId = "0002",
        };
        await spService.GetTableRows(filter);
    }
}
