namespace HE.Investments.Loans.BusinessLogic.Config;

public interface ILoansDocumentSettings
{
    public string ListAlias { get; }

    public string ListTitle { get; }

    public int MaxFileSizeInMegabytes { get; }
}
