namespace HE.InvestmentLoans.WWW.Extensions;

public static class HeaderDictionaryExtension
{
    public static void AddOrUpdate(this IHeaderDictionary headers, string header, string value)
    {
        if (headers.ContainsKey(header))
        {
            headers[header] = value;
        }
        else
        {
            headers.Add(header, value);
        }
    }
}
