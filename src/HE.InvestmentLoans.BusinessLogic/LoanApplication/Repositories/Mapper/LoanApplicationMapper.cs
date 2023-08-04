using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.CompanyStructure;

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
        };
    }

    public static UserAccountDto MapToUserAccountDto(UserAccount userAccount)
    {
        return new UserAccountDto { AccountId = userAccount.AccountId, ContactEmail = userAccount.UserEmail, ContactExternalId = userAccount.UserGlobalId, };
    }

    private static AccountDetailsViewModel MapToAccountDetailsViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new AccountDetailsViewModel { EmailAddress = loanApplicationDto.contactEmailAdress, };
    }

    private static SecurityViewModel MapToSecurityViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new SecurityViewModel
        {
            ChargesDebtCompany = loanApplicationDto.outstandingLegalChargesOrDebt,
            ChargesDebtCompanyInfo = loanApplicationDto.debentureHolder,
            DirLoans = loanApplicationDto.directorLoans,
            DirLoansSub = loanApplicationDto.confirmationDirectorLoansCanBeSubordinated,
            DirLoansSubMore = loanApplicationDto.reasonForDirectorLoanNotSubordinated,
        };
    }

    private static FundingViewModel MapToFundingViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new FundingViewModel
        {
            GrossDevelopmentValue = loanApplicationDto.projectGdv,
            TotalCosts = loanApplicationDto.projectEstimatedTotalCost,
            AbnormalCosts = loanApplicationDto.projectAbnormalCosts,
            AbnormalCostsInfo = loanApplicationDto.projectAbnormalCostsInformation,
            PrivateSectorFunding = loanApplicationDto.privateSectorApproach,
            PrivateSectorFundingResult = loanApplicationDto.privateSectorApproachInformation,
            AdditionalProjects = loanApplicationDto.additionalProjects,
            Refinance = loanApplicationDto.refinanceRepayment,
            RefinanceInfo = loanApplicationDto.refinanceRepaymentDetails,
        };
    }

    private static CompanyStructureViewModel MapToCompanyStructureViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new CompanyStructureViewModel
        {
            Purpose = loanApplicationDto.companyPurpose,
            ExistingCompany = loanApplicationDto.existingCompany,
            HomesBuilt = loanApplicationDto.companyExperience.ToString(),
        };
    }
}
