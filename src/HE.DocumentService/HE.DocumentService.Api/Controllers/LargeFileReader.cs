using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;

namespace HE.DocumentService.Api.Controllers;

public record LargeFile(string Name, Stream Content);

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

    private LargeFile? CreateFile(FileMultipartSection fileSection)
    {
        if (fileSection.FileStream != null)
        {
            var name = fileSection.FileName;
            return new LargeFile(fileSection.FileName, fileSection.FileStream);
        }

        return null;
    }

    private static string GetBoundary(string contentType)
    {
        var elements = contentType.Split(' ');
        var element = elements.Where(entry => entry.StartsWith("boundary=", StringComparison.InvariantCultureIgnoreCase)).First();
        var boundary = element.Substring("boundary=".Length);

        // Remove quotes
        if (boundary.Length >= 2 && boundary[0] == '"' &&
            boundary[boundary.Length - 1] == '"')
        {
            boundary = boundary.Substring(1, boundary.Length - 2);
        }

        return boundary;
    }
}
