using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure;

public class CompanyStructureEntity
{
    public CompanyStructureEntity(LoanApplicationId loanApplicationId, CompanyPurpose? purpose)
    {
        LoanApplicationId = loanApplicationId;
        Purpose = purpose;
    }

    public LoanApplicationId LoanApplicationId { get; }

    public CompanyPurpose? Purpose { get; private set; }

    public OrganisationMoreInformation? MoreInformation { get; private set; }

    public OrganisationMoreInformationFile? MoreMoreInformationFile { get; private set; }

    public HomesBuilt? HomesBuilt { get; private set; }

    public void ProvideCompanyPurpose(CompanyPurpose? purpose)
    {
        Purpose = purpose;
    }

    public void ProvideMoreInformation(OrganisationMoreInformation? moreInformation)
    {
        MoreInformation = moreInformation;
    }

    public void ProvideFileWithMoreInformation(OrganisationMoreInformationFile? moreInformationFile)
    {
        MoreMoreInformationFile = moreInformationFile;
    }

    public void ProvideHowManyHomesBuilt(HomesBuilt homesBuilt)
    {
        HomesBuilt = homesBuilt;
    }
}
