using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using HE.Investments.Account.Shared.User;
using Microsoft.AspNetCore.Http;
using Microsoft.PowerPlatform.Dataverse.Client;

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

    public async Task<ApplicationProjects> GetAllAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                       ?? throw new NotFoundException(nameof(Project), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(ApplicationProjects), loanApplicationId.ToString());

        return ApplicationProjectsMapper.Map(loanApplicationDto, _timeProvider.Now);
    }

    public async Task SaveAllAsync(ApplicationProjects applicationProjects, UserAccount userAccount, CancellationToken cancellationToken)
    {
        foreach (var project in applicationProjects.Projects)
        {
            await SaveAsync(applicationProjects.LoanApplicationId, project, cancellationToken);
        }
    }

    public async Task<Project> GetByIdAsync(ProjectId projectId, UserAccount userAccount, ProjectFieldsSet projectFieldsSet, CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = ProjectCrmFieldNameMapper.Map(projectFieldsSet);

        var req = new invln_getsinglesitedetailsRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_sitedetailsid = projectId.ToString(),
            invln_fieldstoretrieve = fieldsToRetrieve,
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsinglesitedetailsResponse
                       ?? throw new NotFoundException(nameof(Project), projectId.ToString());

        var siteDetailsDto = CrmResponseSerializer.Deserialize<SiteDetailsDto>(response.invln_sitedetail)
                                 ?? throw new NotFoundException(nameof(Project), projectId.ToString());

        return ProjectEntityMapper.Map(siteDetailsDto, _timeProvider.Now);
    }

    public async Task SaveAsync(LoanApplicationId loanApplicationId, Project projectToSave, CancellationToken cancellationToken)
    {
        if (projectToSave.IsNewlyCreated)
        {
            await CreateNewProject(loanApplicationId, projectToSave, cancellationToken);
        }
        else if (projectToSave.IsSoftDeleted)
        {
            await DeleteProject(projectToSave, cancellationToken);
        }
        else
        {
            await UpdateProject(loanApplicationId, projectToSave, cancellationToken);
        }
    }

    private async Task DeleteProject(Project projectToDelete, CancellationToken cancellationToken)
    {
        var req = new invln_deletesitedetailsRequest { invln_sitedetailsid = projectToDelete.Id.Value.ToString(), };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }

    private async Task UpdateProject(LoanApplicationId loanApplicationId, Project projectToSave, CancellationToken cancellationToken)
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
            projectHasStartDate = projectToSave?.StartDate?.Exists,
            startDate = projectToSave?.StartDate?.Value,
            planningPermissionStatus = projectToSave!.Status.IsProvided() ? PlanningPermissionStatusMapper.Map(projectToSave.PlanningPermissionStatus) : null,
            affordableHousing = projectToSave.AffordableHomes?.Value?.MapToBool(),
            completionStatus = SectionStatusMapper.Map(projectToSave.Status),
        };

        var req = new invln_updatesinglesitedetailsRequest
        {
            invln_sitedetail = CrmResponseSerializer.Serialize(siteDetails),
            invln_loanapplicationid = loanApplicationId.Value.ToString(),
            invln_sitedetailsid = projectToSave.Id.ToString(),
            invln_fieldstoupdate = ProjectCrmFieldNameMapper.Map(ProjectFieldsSet.SaveAllFields),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }

    private async Task CreateNewProject(LoanApplicationId loanApplicationId, Project projectToSave, CancellationToken cancellationToken)
    {
        var siteDetails = new SiteDetailsDto { siteDetailsId = projectToSave.Id.Value.ToString(), };

        var req = new invln_createsinglesitedetailRequest
        {
            invln_sitedetails = CrmResponseSerializer.Serialize(siteDetails),
            invln_loanapplicationid = loanApplicationId.Value.ToString(),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }
}
