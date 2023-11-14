using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.Investments.Common.Extensions;

namespace HE.InvestmentLoans.BusinessLogic.Projects;
internal class ProjectMapper
{
    public static ProjectViewModel MapToViewModel(Project project, LoanApplicationId loanApplicationId)
    {
        var additionalDetailsAreProvided = project.AdditionalDetails.IsProvided();

        var startDate = project.StartDate?.Value;

        return new ProjectViewModel
        {
            ProjectId = project.Id!.Value,
            ProjectName = project.Name?.Value,
            HomesCount = project.HomesCount?.Value,
            HomeTypes = project.HomesTypes?.HomesTypesValue,
            OtherHomeTypes = project.HomesTypes?.OtherHomesTypesValue,
            ProjectType = project.ProjectType?.Value,
            ChargesDebt = project.ChargesDebt?.Exist.MapToCommonResponse(),
            ChargesDebtInfo = project.ChargesDebt?.Info,
            AffordableHomes = project.AffordableHomes?.Value,
            ApplicationId = loanApplicationId.Value,
            PlanningReferenceNumberExists = project.PlanningReferenceNumber?.Exists.MapToCommonResponse(),
            PlanningReferenceNumber = project.PlanningReferenceNumber?.Value,
            LocationOption = project.Coordinates is not null ? ProjectFormOption.Coordinates : project.LandRegistryTitleNumber is not null ? ProjectFormOption.LandRegistryTitleNumber : null,
            LocationCoordinates = project.Coordinates?.Value,
            LocationLandRegistry = project.LandRegistryTitleNumber?.Value,
            Ownership = project.LandOwnership?.ApplicantHasFullOwnership.MapToCommonResponse(),
            PurchaseYear = project.AdditionalDetails?.PurchaseDate.AsDateTime().Year.ToString(CultureInfo.InvariantCulture),
            PurchaseMonth = project.AdditionalDetails?.PurchaseDate.AsDateTime().Month.ToString(CultureInfo.InvariantCulture),
            PurchaseDay = project.AdditionalDetails?.PurchaseDate.AsDateTime().Day.ToString(CultureInfo.InvariantCulture),
            Cost = project.AdditionalDetails?.Cost.ToString(),
            Value = project.AdditionalDetails?.CurrentValue.ToString(),
            Source = additionalDetailsAreProvided ? SourceOfValuationMapper.ToString(project.AdditionalDetails!.SourceOfValuation) : null,
            GrantFundingStatus = project.GrantFundingStatus.IsProvided() ? GrantFundingStatusMapper.ToString(project.GrantFundingStatus!.Value) : null,
            GrantFundingProviderName = project.PublicSectorGrantFunding?.ProviderName?.Value,
            GrantFundingAmount = project.PublicSectorGrantFunding?.Amount?.ToString(),
            GrantFundingName = project.PublicSectorGrantFunding?.GrantOrFundName?.Value,
            GrantFundingPurpose = project.PublicSectorGrantFunding?.Purpose?.Value,
            LoanApplicationStatus = project.LoanApplicationStatus,
            LocalAuthorityId = project.LocalAuthority?.Id.ToString(),
            LocalAuthorityName = project.LocalAuthority?.Name,
            HasEstimatedStartDate = project.StartDate?.Exists.MapToCommonResponse(),

            EstimatedStartDay = startDate.HasValue ? startDate.Value.Day.ToString(CultureInfo.InvariantCulture) : null,
            EstimatedStartMonth = startDate.HasValue ? startDate.Value.Month.ToString(CultureInfo.InvariantCulture) : null,
            EstimatedStartYear = startDate.HasValue ? startDate.Value.Year.ToString(CultureInfo.InvariantCulture) : null,

            PlanningPermissionStatus = PlanningPermissionStatusMapper.MapToString(project.PlanningPermissionStatus),
            Status = project.Status,
        };
    }
}
