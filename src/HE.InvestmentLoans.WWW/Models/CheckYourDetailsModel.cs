using HE.Investments.Account.Contract.Organisation.Queries;

namespace HE.InvestmentLoans.WWW.Models;

public class CheckYourDetailsModel
{
    public OrganizationBasicInformation OrganizationBasicInformation { get; set; }

    public string LoanApplicationContactEmail { get; set; }

    public string LoanApplicationContactName { get; set; }

    public string LoanApplicationContactTelephoneNumber { get; set; }
}
