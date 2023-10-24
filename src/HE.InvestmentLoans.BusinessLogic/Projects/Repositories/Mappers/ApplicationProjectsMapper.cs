using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
internal class ApplicationProjectsMapper
{
    public static ApplicationProjects Map(LoanApplicationDto loanApplicationDto, DateTime now)
    {
        var loanApplicationId = LoanApplicationId.From(loanApplicationDto.loanApplicationId);

        var projectsFromCrm = loanApplicationDto.siteDetailsList.Select(
            projectFromCrm =>
            {
                var startDateExists = projectFromCrm.projectHasStartDate;
                var startDate = startDateExists.IsNotProvided() ? null : startDateExists!.Value ? new StartDate(true, new ProjectDate(projectFromCrm.startDate!.Value)) : new StartDate(false, null);

                return new Project(
                         ProjectId.From(projectFromCrm.siteDetailsId),
                         SectionStatusMapper.Map(projectFromCrm.completionStatus),
                         projectFromCrm.Name.IsProvided() ? new ProjectName(projectFromCrm.Name) : null,
                         startDate,
                         projectFromCrm.numberOfHomes.IsProvided() ? new HomesCount(projectFromCrm.numberOfHomes) : null,
                         projectFromCrm.typeOfHomes.IsProvided() ? new HomesTypes(projectFromCrm.typeOfHomes, projectFromCrm.otherTypeOfHomes) : null,
                         projectFromCrm.typeOfSite.IsProvided() ? new ProjectType(projectFromCrm.typeOfSite) : null,
                         projectFromCrm.haveAPlanningReferenceNumber.IsProvided() ? new PlanningReferenceNumber(projectFromCrm.haveAPlanningReferenceNumber!.Value, projectFromCrm.planningReferenceNumber) : null,
                         projectFromCrm.siteCoordinates.IsProvided() ? new Coordinates(projectFromCrm.siteCoordinates) : null,
                         projectFromCrm.landRegistryTitleNumber.IsProvided() ? new LandRegistryTitleNumber(projectFromCrm.landRegistryTitleNumber) : null,
                         projectFromCrm.siteOwnership.IsProvided() ? new LandOwnership(projectFromCrm.siteOwnership!.Value) : null,
                         AdditionalDetailsMapper.MapFromCrm(projectFromCrm, now),
                         projectFromCrm.IsProvided() ? GrantFundingStatusMapper.FromString(projectFromCrm.publicSectorFunding) : null,
                         PublicSectorGrantFundingMapper.MapFromCrm(projectFromCrm),
                         projectFromCrm.existingLegalCharges.IsProvided() ? new ChargesDebt(projectFromCrm.existingLegalCharges ?? false, projectFromCrm.existingLegalChargesInformation) : null,
                         projectFromCrm.affordableHousing.IsProvided() ? new AffordableHomes(projectFromCrm.affordableHousing.MapToCommonResponse()) : null,
                         ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus),
                         PlanningPermissionStatusMapper.Map(projectFromCrm.planningPermissionStatus));
            });

        return new ApplicationProjects(loanApplicationId, projectsFromCrm);
    }
}
