using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Generic;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories;

public class ApplicationProjectsRepository : IApplicationProjectsRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDateTimeProvider _timeProvider;

    private readonly IOrganizationServiceAsync2 _serviceClient;

    public ApplicationProjectsRepository(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime, IOrganizationServiceAsync2 serviceClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _timeProvider = dateTime;
        _serviceClient = serviceClient;
    }

    public async Task<ApplicationProjects> GetById(
        LoanApplicationId loanApplicationId,
        UserAccount userAccount,
        ProjectFieldsSet projectFieldsSet,
        CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = ProjectCrmFieldNameMapper.Map(projectFieldsSet);

        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),

            // invln_fieldstoretrieve = fieldsToRetrieve, // TODO
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                       ?? throw new NotFoundException(nameof(Project), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(ApplicationProjects), loanApplicationId.ToString());

        var projectsFromCrm = loanApplicationDto.siteDetailsList.Select(
            projectFromCrm => new Project(
                                ProjectId.From(projectFromCrm.siteDetailsId),
                                SectionStatusMapper.Map(projectFromCrm.completionStatus),
                                projectFromCrm.Name.IsProvided() ? new ProjectName(projectFromCrm.Name) : null,
                                projectFromCrm.startDate.IsProvided() ? new StartDate(true, new ProjectDate(projectFromCrm.startDate!.Value)) : new StartDate(false, null),
                                projectFromCrm.numberOfHomes.IsProvided() ? new HomesCount(projectFromCrm.numberOfHomes) : null,
                                projectFromCrm.typeOfHomes.IsProvided() ? new HomesTypes(projectFromCrm.typeOfHomes, projectFromCrm.otherTypeOfHomes) : null,
                                projectFromCrm.typeOfSite.IsProvided() ? new ProjectType(projectFromCrm.typeOfSite) : null,
                                projectFromCrm.haveAPlanningReferenceNumber.IsProvided() ? new PlanningReferenceNumber(projectFromCrm.haveAPlanningReferenceNumber!.Value, projectFromCrm.planningReferenceNumber) : null,
                                projectFromCrm.siteCoordinates.IsProvided() ? new Coordinates(projectFromCrm.siteCoordinates) : null,
                                projectFromCrm.landRegistryTitleNumber.IsProvided() ? new LandRegistryTitleNumber(projectFromCrm.landRegistryTitleNumber) : null,
                                projectFromCrm.siteOwnership.IsProvided() ? new LandOwnership(projectFromCrm.siteOwnership!.Value) : null,
                                AdditionalDetailsMapper.MapFromCrm(projectFromCrm, _timeProvider.Now),
                                projectFromCrm.IsProvided() ? GrantFundingStatusMapper.FromString(projectFromCrm.publicSectorFunding) : null,
                                PublicSectorGrantFundingMapper.MapFromCrm(projectFromCrm),
                                projectFromCrm.existingLegalCharges.IsProvided() ? new ChargesDebt(projectFromCrm.existingLegalCharges ?? false, projectFromCrm.existingLegalChargesInformation) : null,
                                projectFromCrm.affordableHousing.IsProvided() ? new AffordableHomes(projectFromCrm.affordableHousing.MapToCommonResponse()) : null,
                                ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus),
                                PlanningPermissionStatusMapper.Map(projectFromCrm.planningPermissionStatus)));

        return new ApplicationProjects(loanApplicationId, projectsFromCrm);
    }

    public async Task SaveAsync(ApplicationProjects applicationProjects, ProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var projectToSave = applicationProjects.Projects.First(c => c.Id == projectId);

        if (projectToSave.IsNewlyCreated)
        {
            await CreateNewProject(applicationProjects, projectToSave, cancellationToken);
        }
        else if (projectToSave.IsSoftDeleted)
        {
            await DeleteProject(projectToSave, cancellationToken);
        }
        else
        {
            await UpdateProject(applicationProjects, projectId, projectToSave, cancellationToken);
        }
    }

    private async Task DeleteProject(Project projectToDelete, CancellationToken cancellationToken)
    {
        var req = new invln_deletesitedetailsRequest { invln_sitedetailsid = projectToDelete.Id.Value.ToString(), };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }

    private async Task UpdateProject(ApplicationProjects applicationProjects, ProjectId projectId, Project projectToSave, CancellationToken cancellationToken)
    {
        var siteDetails = new SiteDetailsDto
        {
            siteDetailsId = projectToSave.Id.Value.ToString(),
            Name = projectToSave.Name?.Value,
            haveAPlanningReferenceNumber = projectToSave.PlanningReferenceNumber?.Exists,
            planningReferenceNumber = projectToSave.PlanningReferenceNumber?.Value,
            siteCoordinates = projectToSave.Coordinates?.Value,
            landRegistryTitleNumber = projectToSave.LandRegistryTitleNumber?.Value,
            siteOwnership = projectToSave.LandOwnership?.ApplicantHasFullOwnership,
            dateOfPurchase = projectToSave.AdditionalDetails?.PurchaseDate.AsDateTime(),
            siteCost = projectToSave.AdditionalDetails?.Cost.ToString(),
            currentValue = projectToSave.AdditionalDetails?.CurrentValue.ToString(),
            valuationSource =
                projectToSave.AdditionalDetails.IsProvided() ? SourceOfValuationMapper.ToCrmString(projectToSave.AdditionalDetails!.SourceOfValuation) : null!,
            publicSectorFunding =
                projectToSave.GrantFundingStatus.IsProvided() ? GrantFundingStatusMapper.ToString(projectToSave.GrantFundingStatus!.Value) : null,
            whoProvided = projectToSave.PublicSectorGrantFunding?.ProviderName?.Value,
            howMuch = projectToSave.PublicSectorGrantFunding?.Amount?.ToString(),
            nameOfGrantFund = projectToSave.PublicSectorGrantFunding?.GrantOrFundName?.Value,
            reason = projectToSave.PublicSectorGrantFunding?.Purpose?.Value,
            numberOfHomes = projectToSave.HomesCount?.Value,
            typeOfHomes = projectToSave.HomesTypes?.HomesTypesValue,
            otherTypeOfHomes = projectToSave.HomesTypes?.OtherHomesTypesValue,
            typeOfSite = projectToSave.ProjectType?.Value,
            existingLegalCharges = projectToSave.ChargesDebt?.Exist,
            existingLegalChargesInformation = projectToSave.ChargesDebt?.Info,
            numberOfAffordableHomes = projectToSave?.AffordableHomes?.Value,
            startDate = projectToSave?.StartDate?.Value,
            planningPermissionStatus = projectToSave!.Status.IsProvided() ? PlanningPermissionStatusMapper.Map(projectToSave.PlanningPermissionStatus) : null,
            affordableHousing = projectToSave.AffordableHomes?.Value?.MapToBool(),
            completionStatus = SectionStatusMapper.Map(projectToSave.Status),
        };

        var req = new invln_updatesinglesitedetailsRequest
        {
            invln_sitedetail = CrmResponseSerializer.Serialize(siteDetails),
            invln_loanapplicationid = applicationProjects.LoanApplicationId.Value.ToString(),
            invln_fieldstoupdate = string.Join(",", CrmSiteNames()), // TODO to delete
            invln_sitedetailsid = projectId.ToString(),

            // invln_fieldstoupdate = string.Join(",", ProjectCrmFieldNameMapper.Map(ProjectFieldsSet.SaveAllFields)), // TODO to add
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }

    private async Task CreateNewProject(ApplicationProjects applicationProjects, Project projectToSave, CancellationToken cancellationToken)
    {
        var siteDetails = new SiteDetailsDto { siteDetailsId = projectToSave.Id.Value.ToString(), };

        var req = new invln_createsinglesitedetailRequest
        {
            invln_sitedetails = CrmResponseSerializer.Serialize(siteDetails),
            invln_loanapplicationid = applicationProjects.LoanApplicationId.Value.ToString(),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }

    private IEnumerable<string> CrmSiteNames() // TODO to delete
    {
        yield return nameof(invln_SiteDetails.invln_Name).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Haveaplanningreferencenumber).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Planningreferencenumber).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Sitecoordinates).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Landregistrytitlenumber).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Siteownership).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Siteownership).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Dateofpurchase).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Sitecost).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_currentvalue).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Valuationsource).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Publicsectorfunding).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Whoprovided).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Reason).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_HowMuch).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Nameofgrantfund).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Numberofhomes).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Typeofhomes).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_OtherTypeofhomes).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_TypeofSite).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Existinglegalcharges).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Existinglegalchargesinformation).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_Affordablehousing).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_startdate).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_planningpermissionstatus).ToLowerInvariant();
        yield return nameof(invln_SiteDetails.invln_completionstatus).ToLowerInvariant();
    }
}
