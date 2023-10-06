using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Generic;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
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

    public ApplicationProjects GetAll(LoanApplicationId loanApplicationId, UserAccount userAccount)
    {
        var loanApplication = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationEntity>(loanApplicationId.ToString())
            ?? throw new NotFoundException(nameof(LoanApplicationEntity).ToString(), loanApplicationId.ToString());

        return loanApplication.ApplicationProjects;
    }

    public LoanApplicationViewModel LegacyDeleteProject(Guid loanApplicationId, Guid projectId)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationId.ToString())!;

        var projectToDelete = loanApplicationSessionModel.Sites.FirstOrDefault(p => p.Id == projectId) ?? throw new NotFoundException(nameof(SiteViewModel).ToString(), projectId);

        loanApplicationSessionModel!.SetTimestamp(_timeProvider.Now);
        loanApplicationSessionModel!.Sites.Remove(projectToDelete);

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationId.ToString(), loanApplicationSessionModel);

        return loanApplicationSessionModel;
    }

    public async Task<ApplicationProjects> GetById(LoanApplicationId loanApplicationId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = ProjectCrmFieldNameMapper.Map(projectFieldsSet);

        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            // invln_fieldstoretrieve = fieldsToRetrieve,
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                      ?? throw new NotFoundException(nameof(Project), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(ApplicationProjects), loanApplicationId.ToString());

        var projectsFromCrm = loanApplicationDto.siteDetailsList.Select(
            projectFromCrm => new Project(
                                ProjectId.From(projectFromCrm.siteDetailsId),
                                projectFromCrm.Name.IsProvided() ? new ProjectName(projectFromCrm.Name) : null,
                                null,
                                projectFromCrm.haveAPlanningReferenceNumber.IsProvided() ? new PlanningReferenceNumber(projectFromCrm.haveAPlanningReferenceNumber!.Value, projectFromCrm.planningReferenceNumber) : null,
                                projectFromCrm.siteCoordinates.IsProvided() ? new Coordinates(projectFromCrm.siteCoordinates) : null,
                                projectFromCrm.landRegistryTitleNumber.IsProvided() ? new LandRegistryTitleNumber(projectFromCrm.landRegistryTitleNumber) : null,
                                projectFromCrm.siteOwnership.IsProvided() ? new LandOwnership(projectFromCrm.siteOwnership!.Value) : null,
                                AdditionalDetailsMapper.MapFromCrm(projectFromCrm, _timeProvider.Now),
                                projectFromCrm.IsProvided() ? GrantFundingStatusMapper.FromString(projectFromCrm.publicSectorFunding) : null,
                                PublicSectorGrantFundingMapper.MapFromCrm(projectFromCrm),
                                ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus)));

        return new ApplicationProjects(loanApplicationId, projectsFromCrm);
    }

    public async Task<Project> GetById(LoanApplicationId loanApplicationId, ProjectId projectId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = ProjectCrmFieldNameMapper.Map(projectFieldsSet);

        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_fieldstoretrieve = fieldsToRetrieve,
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                      ?? throw new NotFoundException(nameof(Project), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(Project), projectId.ToString());

        var projectFromCrm = loanApplicationDto.siteDetailsList.FirstOrDefault(c => c.siteDetailsId == projectId.Value.ToString())
                                ?? throw new NotFoundException(nameof(Project), projectId.ToString());

        return new Project
        {
            NameLegacy = projectFromCrm.siteName,
            ManyHomes = projectFromCrm.numberOfHomes,
            TypeHomes = projectFromCrm.typeOfHomes,
            TypeHomesOther = projectFromCrm.otherTypeOfHomes,
            Type = projectFromCrm.typeOfSite,
            PlanningRef = projectFromCrm.haveAPlanningReferenceNumber,
            PlanningRefEnter = projectFromCrm.planningReferenceNumber,
            PurchaseDate = projectFromCrm.dateOfPurchase,
            Cost = projectFromCrm.siteCost,
            Value = projectFromCrm.currentValue,
            Source = projectFromCrm.valuationSource,
            GrantFunding = projectFromCrm.publicSectorFunding,
            GrantFundingAmount = projectFromCrm.howMuch,
            GrantFundingName = projectFromCrm.nameOfGrantFund,
            GrantFundingPurpose = projectFromCrm.reason,
            ChargesDebt = projectFromCrm.existingLegalCharges,
            ChargesDebtInfo = projectFromCrm.existingLegalChargesInformation,
            AffordableHomes = projectFromCrm.numberOfAffordableHomes,
        };
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
        var req = new invln_deletesitedetailsRequest
        {
            invln_sitedetailsid = projectToDelete.Id.Value.ToString(),
        };

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
            valuationSource = projectToSave.AdditionalDetails.IsProvided() ? SourceOfValuationMapper.ToString(projectToSave.AdditionalDetails!.SourceOfValuation) : null!,
            publicSectorFunding = projectToSave.GrantFundingStatus.IsProvided() ? GrantFundingStatusMapper.ToString(projectToSave.GrantFundingStatus!.Value) : null,
            whoProvided = projectToSave.PublicSectorGrantFunding?.ProviderName?.Value,
            howMuch = projectToSave.PublicSectorGrantFunding?.Amount?.ToString(),
            nameOfGrantFund = projectToSave.PublicSectorGrantFunding?.GrantOrFundName?.Value,
            reason = projectToSave.PublicSectorGrantFunding?.Purpose?.Value,
        };

        var req = new invln_updatesinglesitedetailsRequest
        {
            invln_sitedetail = CrmResponseSerializer.Serialize(siteDetails),
            invln_loanapplicationid = applicationProjects.LoanApplicationId.Value.ToString(),
            invln_fieldstoupdate = string.Join(",", ProjectCrmFieldNameMapper.Map(ProjectFieldsSet.SaveAllFields)),
            invln_sitedetailsid = projectId.ToString(),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }

    private async Task CreateNewProject(ApplicationProjects applicationProjects, Project projectToSave, CancellationToken cancellationToken)
    {
        var siteDetails = new SiteDetailsDto
        {
            siteDetailsId = projectToSave.Id.Value.ToString(),
        };

        var req = new invln_createsinglesitedetailRequest
        {
            invln_sitedetails = CrmResponseSerializer.Serialize(siteDetails),
            invln_loanapplicationid = applicationProjects.LoanApplicationId.Value.ToString(),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }
}
