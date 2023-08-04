using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure;

public class CompanyStructureEntity
{
    public CompanyStructureEntity(LoanApplicationId loanApplicationId, Providable<CompanyPurpose> purpose)
    {
        LoanApplicationId = loanApplicationId;
        Purpose = purpose;
    }

    public LoanApplicationId LoanApplicationId { get; }

    public Providable<CompanyPurpose> Purpose { get; private set; }

    public Providable<OrganisationMoreInformation> MoreInformation { get; private set; }

    public Providable<OrganisationMoreInformationFile> MoreMoreInformationFile { get; private set; }

    public Providable<HomesBuilt> HomesBuilt { get; private set; }

    public void ProvideCompanyPurpose(Providable<CompanyPurpose> purpose)
    {
        Purpose = purpose;
    }

    public void ProvideMoreInformation(Providable<OrganisationMoreInformation> moreInformation)
    {
        MoreInformation = moreInformation;
    }

    public void ProvideFileWithMoreInformation(Providable<OrganisationMoreInformationFile> moreInformationFile)
    {
        MoreMoreInformationFile = moreInformationFile;
    }

    public void ProvideHowManyHomesBuilt(Providable<HomesBuilt> homesBuilt)
    {
        HomesBuilt = homesBuilt;
    }
}
