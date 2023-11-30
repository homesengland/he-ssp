namespace HE.Investments.Loans.Contract.Documents;

public class LoansFileTableRow
{
    public string FileName { get; set; }

    public string FolderPath { get; set; }

    public string Editor { get; set; }

    public DateTime Modified { get; set; }

    public string? Creator { get; set; }
}
