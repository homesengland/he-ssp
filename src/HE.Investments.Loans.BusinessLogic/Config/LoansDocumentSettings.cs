namespace HE.Investments.Loans.BusinessLogic.Config;

public class LoansDocumentSettings : ILoansDocumentSettings
{
    public string ListAlias { get; set; }

    public string ListTitle { get; set; }

    public int MaxFileSizeInMegabytes { get; set; }
}
