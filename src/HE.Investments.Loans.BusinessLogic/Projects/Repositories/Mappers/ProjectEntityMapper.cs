using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Common.Extensions;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;

internal class ProjectEntityMapper
{
    public static Project Map(SiteDetailsDto siteDetailsDto, DateTime now)
    {
        var startDateExists = siteDetailsDto.projectHasStartDate;
        var startDate = startDateExists.IsNotProvided() ? null :
            startDateExists!.Value ? new StartDate(true, new ProjectDate(siteDetailsDto.startDate!.Value)) : new StartDate(false, null);

        return new Project(
            ProjectId.From(siteDetailsDto.siteDetailsId),
            SectionStatusMapper.Map(siteDetailsDto.completionStatus),
            siteDetailsDto.Name.IsProvided() ? new ProjectName(siteDetailsDto.Name) : null,
            startDate,
            siteDetailsDto.numberOfHomes.IsProvided() ? new HomesCount(siteDetailsDto.numberOfHomes) : null,
            siteDetailsDto.typeOfHomes.IsProvided() ? new HomesTypes(siteDetailsDto.typeOfHomes, siteDetailsDto.otherTypeOfHomes) : null,
            siteDetailsDto.typeOfSite.IsProvided() ? new ProjectType(siteDetailsDto.typeOfSite) : null,
            siteDetailsDto.haveAPlanningReferenceNumber.IsProvided() ? new PlanningReferenceNumber(siteDetailsDto.haveAPlanningReferenceNumber!.Value, siteDetailsDto.planningReferenceNumber) : null,
            siteDetailsDto.siteCoordinates.IsProvided() ? new Coordinates(siteDetailsDto.siteCoordinates) : null,
            siteDetailsDto.landRegistryTitleNumber.IsProvided() ? new LandRegistryTitleNumber(siteDetailsDto.landRegistryTitleNumber) : null,
            siteDetailsDto.siteOwnership.IsProvided() ? new LandOwnership(siteDetailsDto.siteOwnership!.Value) : null,
            AdditionalDetailsMapper.MapFromCrm(siteDetailsDto, now),
            siteDetailsDto.IsProvided() ? GrantFundingStatusMapper.FromString(siteDetailsDto.publicSectorFunding) : null,
            PublicSectorGrantFundingMapper.MapFromCrm(siteDetailsDto),
            siteDetailsDto.existingLegalCharges.IsProvided() ? new ChargesDebt(siteDetailsDto.existingLegalCharges ?? false, siteDetailsDto.existingLegalChargesInformation) : null,
            siteDetailsDto.affordableHousing.IsProvided() ? new AffordableHomes(siteDetailsDto.affordableHousing.MapToCommonResponse()) : null,
            ApplicationStatusMapper.MapToPortalStatus(siteDetailsDto.loanApplicationStatus),
            PlanningPermissionStatusMapper.Map(siteDetailsDto.planningPermissionStatus),
            LocalAuthorityMapper.MapToLocalAuthority(siteDetailsDto.localAuthority));
    }
}
