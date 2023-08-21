using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure;

public class CompanyStructureEntity
{
    public CompanyStructureEntity(
                    LoanApplicationId loanApplicationId,
                    CompanyPurpose? purpose,
                    OrganisationMoreInformation? moreInformation,
                    OrganisationMoreInformationFile? moreInformationFile,
                    HomesBuilt? homesBuilt,
                    SectionStatus status)
    {
        LoanApplicationId = loanApplicationId;
        Purpose = purpose;
        MoreInformation = moreInformation;
        MoreInformationFile = moreInformationFile;
        HomesBuilt = homesBuilt;
        Status = status;
    }

    public CompanyPurpose? Purpose { get; private set; }

    public OrganisationMoreInformation? MoreInformation { get; private set; }

    public OrganisationMoreInformationFile? MoreInformationFile { get; private set; }

    public HomesBuilt? HomesBuilt { get; private set; }

    public SectionStatus Status { get; private set; }

    public LoanApplicationId LoanApplicationId { get; }

    public void ProvideCompanyPurpose(CompanyPurpose? purpose)
    {
        Purpose = purpose;
        UnCompleteSection();
    }

    public void ProvideMoreInformation(OrganisationMoreInformation? moreInformation)
    {
        MoreInformation = moreInformation;
        UnCompleteSection();
    }

    public void ProvideFileWithMoreInformation(OrganisationMoreInformationFile? moreInformationFile)
    {
        MoreInformationFile = moreInformationFile;
        UnCompleteSection();
    }

    public void ProvideHowManyHomesBuilt(HomesBuilt? homesBuilt)
    {
        HomesBuilt = homesBuilt;
        UnCompleteSection();
    }

    public void CompleteSection()
    {
        Status = SectionStatus.Completed;
    }

    public void UnCompleteSection()
    {
        Status = SectionStatus.InProgress;
    }
}
