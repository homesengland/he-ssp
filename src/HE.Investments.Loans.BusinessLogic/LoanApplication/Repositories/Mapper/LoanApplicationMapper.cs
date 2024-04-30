using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
using HE.Investments.Loans.BusinessLogic.ViewModel;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure;
using HE.Investments.Loans.Contract.Funding;
using HE.Investments.Loans.Contract.Security;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;

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
            Projects = ApplicationProjectsMapper.Map(loanApplicationDto).Projects.Select(p => ProjectMapper.MapToViewModel(p, LoanApplicationId.From(loanApplicationDto.loanApplicationId))),
        };

        model.SetTimestamp(loanApplicationDto.LastModificationOn ?? loanApplicationDto.createdOn);

        return model;
    }

    public static UserAccountDto MapToUserAccountDto(UserAccount userAccount, UserProfileDetails userProfileDetails)
    {
        return new UserAccountDto
        {
            AccountId = userAccount.SelectedOrganisationId().Value,
            ContactEmail = userAccount.UserEmail,
            ContactExternalId = userAccount.UserGlobalId.ToString(),
            ContactFirstName = userProfileDetails.FirstName?.ToString(),
            ContactLastName = userProfileDetails.LastName?.ToString(),
            ContactTelephoneNumber = userProfileDetails.TelephoneNumber?.ToString(),
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
            Status = SectionStatusMapper.Map(loanApplicationDto.SecurityDetailsCompletionStatus),
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
            Status = SectionStatusMapper.Map(loanApplicationDto.FundingDetailsCompletionStatus),
        };
    }

    private static CompanyStructureViewModel MapToCompanyStructureViewModel(LoanApplicationDto loanApplicationDto)
    {
        return new CompanyStructureViewModel
        {
            LoanApplicationId = Guid.Parse(loanApplicationDto.loanApplicationId),
            Purpose = loanApplicationDto.companyPurpose.MapToCommonResponse(),
            OrganisationMoreInformation = loanApplicationDto.existingCompany,
            HomesBuilt = loanApplicationDto.companyExperience?.ToString(CultureInfo.InvariantCulture),
            Status = SectionStatusMapper.Map(loanApplicationDto.CompanyStructureAndExperienceCompletionStatus),
        };
    }
}
