using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Projects;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.Funding;
using HE.InvestmentLoans.Contract.Security;
using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;

public static class LoanApplicationMapper
{
    public static LoanApplicationViewModel FromCrmDto(LoanApplicationDto loanApplicationDto, DateTime now)
    {
        var model = new LoanApplicationViewModel
        {
            ID = Guid.Parse(loanApplicationDto.loanApplicationId),
            Status = ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus),
            Purpose = FundingPurposeMapper.Map(loanApplicationDto.fundingReason),
            Company = MapToCompanyStructureViewModel(loanApplicationDto),
            Funding = MapToFundingViewModel(loanApplicationDto),
            Security = MapToSecurityViewModel(loanApplicationDto),
            ReferenceNumber = loanApplicationDto.name,
            Projects = ApplicationProjectsMapper.Map(loanApplicationDto, now).Projects.Select(p => ProjectMapper.MapToViewModel(p, LoanApplicationId.From(loanApplicationDto.loanApplicationId))),
        };

        model.SetTimestamp(loanApplicationDto.LastModificationOn ?? loanApplicationDto.createdOn);

        return model;
    }

    public static UserAccountDto MapToUserAccountDto(UserAccount userAccount, UserDetails userDetails)
    {
        return new UserAccountDto
        {
            AccountId = (Guid)userAccount.AccountId!,
            ContactEmail = userAccount.UserEmail,
            ContactExternalId = userAccount.UserGlobalId.ToString(),
            ContactFirstName = userDetails.FirstName?.ToString(),
            ContactLastName = userDetails.LastName?.ToString(),
            ContactTelephoneNumber = userDetails.TelephoneNumber?.ToString(),
        };
    }

    private static SecurityViewModel MapToSecurityViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new SecurityViewModel
        {
            LoanApplicationId = Guid.Parse(loanApplicationDto.loanApplicationId),
            ChargesDebtCompany = loanApplicationDto.outstandingLegalChargesOrDebt.MapToCommonResponse(),
            ChargesDebtCompanyInfo = loanApplicationDto.debentureHolder,
            DirLoans = loanApplicationDto.directorLoans.MapToCommonResponse(),
            DirLoansSub = loanApplicationDto.confirmationDirectorLoansCanBeSubordinated.MapToCommonResponse(),
            DirLoansSubMore = loanApplicationDto.reasonForDirectorLoanNotSubordinated,
            State = SectionStatusMapper.Map(loanApplicationDto.SecurityDetailsCompletionStatus),
        };
    }

    private static FundingViewModel MapToFundingViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new FundingViewModel
        {
            LoanApplicationId = Guid.Parse(loanApplicationDto.loanApplicationId),
            GrossDevelopmentValue = loanApplicationDto.projectGdv?.ToString("0.##", CultureInfo.InvariantCulture),
            TotalCosts = loanApplicationDto.projectEstimatedTotalCost?.ToString("0.##", CultureInfo.InvariantCulture),
            AbnormalCosts = loanApplicationDto.projectAbnormalCosts.MapToCommonResponse(),
            AbnormalCostsInfo = loanApplicationDto.projectAbnormalCostsInformation,
            PrivateSectorFunding = loanApplicationDto.privateSectorApproach.MapToCommonResponse(),
            PrivateSectorFundingResult = loanApplicationDto.privateSectorApproachInformation,
            AdditionalProjects = loanApplicationDto.additionalProjects.MapToCommonResponse(),
            Refinance = loanApplicationDto.refinanceRepayment,
            RefinanceInfo = loanApplicationDto.refinanceRepaymentDetails,
            State = SectionStatusMapper.Map(loanApplicationDto.FundingDetailsCompletionStatus),
        };
    }

    private static CompanyStructureViewModel MapToCompanyStructureViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new CompanyStructureViewModel
        {
            LoanApplicationId = Guid.Parse(loanApplicationDto.loanApplicationId),
            Purpose = loanApplicationDto.companyPurpose.MapToCommonResponse(),
            OrganisationMoreInformation = loanApplicationDto.existingCompany,
            HomesBuilt = loanApplicationDto.companyExperience.ToString(),
            State = SectionStatusMapper.Map(loanApplicationDto.CompanyStructureAndExperienceCompletionStatus),
        };
    }
}
