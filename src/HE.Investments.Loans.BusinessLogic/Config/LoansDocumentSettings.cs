using Microsoft.Extensions.Configuration;

namespace HE.Investments.Loans.BusinessLogic.Config;

public class LoansDocumentSettings : ILoansDocumentSettings
{
    public LoansDocumentSettings(IConfiguration configuration)
    {
        ListAlias = configuration.GetValue<string>("AppConfiguration:DocumentService:ListAlias") ?? string.Empty;
        ListTitle = configuration.GetValue<string>("AppConfiguration:DocumentService:ListTitle") ?? string.Empty;
        MaxFileSizeInMegabytes = configuration.GetValue<int?>("AppConfiguration:DocumentService:MaxFileSizeInMegabytes") ?? 20;
    }

    public string ListAlias { get; }

    public string ListTitle { get; }

    public int MaxFileSizeInMegabytes { get; }
}
