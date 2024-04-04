namespace HE.Investments.Loans.BusinessLogic.Config;

public interface ILoanAppConfig
{
    public string? LoansEnquiriesTelephoneNumber { get; set; }

    public string? LoansEnquiriesEmail { get; set; }

    public string? FundingSupportEmail { get; set; }
}
