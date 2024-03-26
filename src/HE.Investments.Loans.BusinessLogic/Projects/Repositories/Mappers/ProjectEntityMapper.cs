extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using LocalAuthorityMapper = Org::HE.Investments.Organisation.LocalAuthorities.Mappers.LocalAuthorityMapper;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;

internal static class ProjectEntityMapper
{
    public static Project Map(SiteDetailsDto siteDetailsDto, DateTime now)
    {
        var startDate = GetStartDate(siteDetailsDto);

        return new Project(
            ProjectId.From(siteDetailsDto.siteDetailsId),
            siteDetailsDto.frontDoorSiteId.IsProvided() ? new FrontDoorSiteId(siteDetailsDto.frontDoorSiteId) : null,
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
            LocalAuthorityMapper.MapToLocalAuthority(siteDetailsDto.localAuthority?.onsCode, siteDetailsDto.localAuthority?.name));
    }

    private static StartDate? GetStartDate(SiteDetailsDto siteDetailsDto)
    {
        var startDateExists = siteDetailsDto.projectHasStartDate;

        if (startDateExists.IsNotProvided())
        {
            return null;
        }

        if (startDateExists!.Value)
        {
            return new StartDate(true, new ProjectDate(siteDetailsDto.startDate!.Value));
        }

        return new StartDate(false, null);
    }
}
