using System.Globalization;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.BusinessLogic.PrefillData.Data;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Projects.ViewModels;

namespace HE.Investments.Loans.BusinessLogic.Projects;

internal static class ProjectMapper
{
    public static ProjectViewModel MapToViewModel(Project project, LoanApplicationId loanApplicationId, LoanProjectPrefillData? projectPrefillData = null)
    {
        var additionalDetailsAreProvided = project.AdditionalDetails.IsProvided();
        var locationOption = DetermineLocationOption(project);

        return new ProjectViewModel
        {
            ProjectId = project.Id.Value,
            ProjectName = project.Name?.Value ?? projectPrefillData?.Name,
            HomesCount = project.HomesCount?.Value ?? projectPrefillData?.NumberOfHomes?.ToString(CultureInfo.InvariantCulture),
            HomeTypes = project.HomesTypes?.HomesTypesValue,
            OtherHomeTypes = project.HomesTypes?.OtherHomesTypesValue,
            ProjectType = project.ProjectType?.Value,
            ChargesDebt = project.ChargesDebt?.Exist.MapToCommonResponse(),
            ChargesDebtInfo = project.ChargesDebt?.Info,
            AffordableHomes = project.AffordableHomes?.Value,
            ApplicationId = loanApplicationId.Value,
            PlanningReferenceNumberExists = project.PlanningReferenceNumber?.Exists.MapToCommonResponse(),
            PlanningReferenceNumber = project.PlanningReferenceNumber?.Value,
            LocationOption = locationOption,
            LocationCoordinates = project.Coordinates?.Value,
            LocationLandRegistry = project.LandRegistryTitleNumber?.Value,
            Ownership = project.LandOwnership?.ApplicantHasFullOwnership.MapToCommonResponse(),
            PurchaseDate = DateDetails.FromDateTime(project.AdditionalDetails?.PurchaseDate?.Value),
            Cost = project.AdditionalDetails?.Cost.ToString(),
            Value = project.AdditionalDetails?.CurrentValue.ToString(),
            Source = additionalDetailsAreProvided ? SourceOfValuationMapper.ToString(project.AdditionalDetails!.SourceOfValuation) : null,
            GrantFundingStatus = project.GrantFundingStatus.IsProvided() ? GrantFundingStatusMapper.ToString(project.GrantFundingStatus!.Value) : null,
            GrantFundingProviderName = project.PublicSectorGrantFunding?.ProviderName?.Value,
            GrantFundingAmount = project.PublicSectorGrantFunding?.Amount?.ToString(),
            GrantFundingName = project.PublicSectorGrantFunding?.GrantOrFundName?.Value,
            GrantFundingPurpose = project.PublicSectorGrantFunding?.Purpose?.Value,
            LoanApplicationStatus = project.LoanApplicationStatus,
            LocalAuthorityId = project.LocalAuthority?.Code.ToString(),
            LocalAuthorityName = project.LocalAuthority?.Name,
            HasEstimatedStartDate = project.StartDate?.Value.HasValue.MapToCommonResponse(),
            StartDate = DateDetails.FromDateTime(project.StartDate?.Value),
            PlanningPermissionStatus = PlanningPermissionStatusMapper.MapToString(project.PlanningPermissionStatus ?? projectPrefillData?.PlanningPermissionStatus),
            Status = project.Status,
        };
    }

    private static string? DetermineLocationOption(Project project)
    {
        string? locationOption = null;
        if (project.Coordinates is not null)
        {
            locationOption = ProjectFormOption.Coordinates;
        }
        else if (project.LandRegistryTitleNumber is not null)
        {
            locationOption = ProjectFormOption.LandRegistryTitleNumber;
        }

        return locationOption;
    }
}
