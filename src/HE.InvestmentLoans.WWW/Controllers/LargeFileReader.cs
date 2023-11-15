using System.Net.Http.Headers;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using Microsoft.AspNetCore.WebUtilities;

namespace HE.InvestmentLoans.WWW.Controllers;

public class LargeFileReader
{
    public async IAsyncEnumerable<LargeFile> Read(HttpRequest request)
    {
        var x = MediaTypeHeaderValue.Parse(request.ContentType);
        var boundary = GetBoundary(x.ToString());
        var multipartReader = new MultipartReader(boundary, request.Body);

        var section = await multipartReader.ReadNextSectionAsync();

        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                var file = CreateFile(fileSection);
                if (file != null)
                {
                    yield return file;
                }
            }

            section = await multipartReader.ReadNextSectionAsync();
        }
    }

    private static string GetBoundary(string contentType)
    {
        var elements = contentType.Split(' ');
        var element = elements.Where(entry => entry.StartsWith("boundary=", StringComparison.InvariantCultureIgnoreCase)).First();
        var boundary = element["boundary=".Length..];

        // Remove quotes
        if (boundary.Length >= 2 && boundary[0] == '"' &&
            boundary[^1] == '"')
        {
            boundary = boundary[1..^1];
        }

        return boundary;
    }

    private LargeFile CreateFile(FileMultipartSection fileSection)
    {
        if (fileSection.FileStream != null)
        {
            return new LargeFile(fileSection.FileName, fileSection.FileStream);
        }

        return null;
    }
}
