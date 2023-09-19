using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.Funding;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;

public static class LoanApplicationMapper
{
    public static LoanApplicationViewModel Map(LoanApplicationDto loanApplicationDto)
    {
        return new LoanApplicationViewModel
        {
            ID = Guid.Parse(loanApplicationDto.loanApplicationId),
            Purpose = FundingPurposeMapper.Map(loanApplicationDto.fundingReason),
            Company = MapToCompanyStructureViewModel(loanApplicationDto),
            Funding = MapToFundingViewModel(loanApplicationDto),
            Security = MapToSecurityViewModel(loanApplicationDto),
            Account = MapToAccountDetailsViewModel(loanApplicationDto),
            ReferenceNumber = loanApplicationDto.name,
        };
    }

    public static UserAccountDto MapToUserAccountDto(UserAccount userAccount)
    {
        return new UserAccountDto
        {
            AccountId = (Guid)userAccount.AccountId!,
            ContactEmail = userAccount.UserEmail,
            ContactExternalId = userAccount.UserGlobalId.ToString(),
            ContactFirstName = userAccount.FirstName,
            ContactLastName = userAccount.LastName,
            ContactTelephoneNumber = userAccount.TelephoneNumber,
        };
    }

    private static AccountDetailsViewModel MapToAccountDetailsViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new AccountDetailsViewModel { EmailAddress = loanApplicationDto.contactEmailAdress, };
    }

    private static SecurityViewModel MapToSecurityViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new SecurityViewModel
        {
            ChargesDebtCompany = loanApplicationDto.outstandingLegalChargesOrDebt.MapToCommonResponse(),
            ChargesDebtCompanyInfo = loanApplicationDto.debentureHolder,
            DirLoans = loanApplicationDto.directorLoans.MapToCommonResponse(),
            DirLoansSub = loanApplicationDto.confirmationDirectorLoansCanBeSubordinated.MapToCommonResponse(),
            DirLoansSubMore = loanApplicationDto.reasonForDirectorLoanNotSubordinated,
        };
    }

    private static FundingViewModel MapToFundingViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new FundingViewModel
        {
            GrossDevelopmentValue = loanApplicationDto.projectGdv,
            TotalCosts = loanApplicationDto.projectEstimatedTotalCost,
            AbnormalCosts = loanApplicationDto.projectAbnormalCosts.MapToCommonResponse(),
            AbnormalCostsInfo = loanApplicationDto.projectAbnormalCostsInformation,
            PrivateSectorFunding = loanApplicationDto.privateSectorApproach.MapToCommonResponse(),
            PrivateSectorFundingResult = loanApplicationDto.privateSectorApproachInformation,
            AdditionalProjects = loanApplicationDto.additionalProjects.MapToCommonResponse(),
            Refinance = loanApplicationDto.refinanceRepayment,
            RefinanceInfo = loanApplicationDto.refinanceRepaymentDetails,
        };
    }

    private static CompanyStructureViewModel MapToCompanyStructureViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new CompanyStructureViewModel
        {
            Purpose = loanApplicationDto.companyPurpose.MapToCommonResponse(),
            OrganisationMoreInformation = loanApplicationDto.existingCompany,
            HomesBuilt = loanApplicationDto.companyExperience.ToString(),
            State = SectionStatusMapper.Map(loanApplicationDto.CompanyStructureAndExperienceCompletionStatus),
        };
    }
}
