using System.Reflection;

namespace He.Investments.AspNetCore.UI.Common.Utils;

internal static class ResourceReader
{
    internal static string Read(string embeddedFileName)
    {
        var assembly = typeof(ResourceReader).GetTypeInfo().Assembly;
        var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFileName, StringComparison.CurrentCultureIgnoreCase));

        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("Could not load manifest resource stream.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
