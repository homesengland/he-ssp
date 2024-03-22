using Microsoft.Extensions.Configuration;

namespace HE.Investments.Loans.BusinessLogic.Config;

public class LoanAppConfig : ILoanAppConfig
{
    public LoanAppConfig(IConfiguration configuration)
    {
        LoansEnquiriesTelephoneNumber = configuration.GetValue<string>("AppConfiguration:LoansEnquiriesTelephoneNumber") ?? string.Empty;
        LoansEnquiriesEmail = configuration.GetValue<string>("AppConfiguration:LoansEnquiriesEmail") ?? string.Empty;
    }

    public string? LoansEnquiriesTelephoneNumber { get; set; }

    public string? LoansEnquiriesEmail { get; set; }
}
