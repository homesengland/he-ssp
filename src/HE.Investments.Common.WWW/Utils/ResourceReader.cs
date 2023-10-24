using System.Reflection;

namespace HE.Investments.Common.WWW.Utils;

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
