using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Extensions;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.DocumentService.Models;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Constants;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.FeatureManagement;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository, ICanSubmitLoanApplication, IChangeApplicationStatus
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IEventDispatcher _eventDispatcher;

    private readonly IFileApplicationRepository _fileRepository;

    private readonly ILoansDocumentSettings _documentSettings;

    private readonly IFeatureManager _featureManager;

    public LoanApplicationRepository(
        IOrganizationServiceAsync2 serviceClient,
        IEventDispatcher eventDispatcher,
        IFileApplicationRepository fileRepository,
        ILoansDocumentSettings documentSettings,
        IFeatureManager featureManager)
    {
        _serviceClient = serviceClient;
        _eventDispatcher = eventDispatcher;
        _fileRepository = fileRepository;
        _documentSettings = documentSettings;
        _featureManager = featureManager;
    }

    public async Task<bool> IsExist(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_fieldstoretrieve = nameof(invln_Loanapplication.invln_LoanapplicationId).ToLowerInvariant(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse;
        if (response.IsNotProvided())
        {
            return false;
        }

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response!.invln_loanapplication)?.FirstOrDefault();
        if (loanApplicationDto.IsNotProvided() || loanApplicationDto!.loanApplicationId.IsNotProvided())
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsExist(LoanApplicationName loanApplicationName, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_checkifloanapplicationwithgivennameexistsRequest
        {
            invln_loanname = loanApplicationName.Value,
            invln_organisationid = userAccount.Organisation?.OrganisationId.ToString(),
        };

        var response = (invln_checkifloanapplicationwithgivennameexistsResponse)await _serviceClient.ExecuteAsync(req, cancellationToken);
        return response.invln_loanexists;
    }

    public async Task<LoanApplicationEntity> GetLoanApplication(LoanApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = id.ToString(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                       ?? throw new NotFoundException(nameof(LoanApplicationEntity), id.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(LoanApplicationEntity), id.ToString());

        var externalStatus = ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus);

        var projects = loanApplicationDto.siteDetailsList.Select(
            site => new ProjectBasicData(
                SectionStatusMapper.Map(site.completionStatus),
                ProjectId.From(site.siteDetailsId),
                site.numberOfHomes.IsProvided() ? new HomesCount(site.numberOfHomes) : null,
                site.Name.IsProvided() ? new ProjectName(site.Name) : ProjectName.Default));

        return new LoanApplicationEntity(
            id,
            LoanApplicationName.CreateOrDefault(loanApplicationDto.ApplicationName),
            userAccount,
            externalStatus,
            FundingPurposeMapper.Map(loanApplicationDto.fundingReason),
            loanApplicationDto.createdOn,
            loanApplicationDto.LastModificationOn,
            loanApplicationDto.dateSubmitted,
            loanApplicationDto.lastModificationByName,
            new LoanApplicationSection(SectionStatusMapper.Map(loanApplicationDto.CompanyStructureAndExperienceCompletionStatus)),
            new LoanApplicationSection(SectionStatusMapper.Map(loanApplicationDto.SecurityDetailsCompletionStatus)),
            new LoanApplicationSection(SectionStatusMapper.Map(loanApplicationDto.FundingDetailsCompletionStatus)),
            new ProjectsSection(projects),
            loanApplicationDto.name,
            string.IsNullOrWhiteSpace(loanApplicationDto.frontDoorProjectId) ? null : new FrontDoorProjectId(loanApplicationDto.frontDoorProjectId));
    }

    public async Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getloanapplicationsforaccountandcontactRequest()
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getloanapplicationsforaccountandcontactResponse
                       ?? throw new NotFoundException("Applications list", userAccount.ToString());

        var loanApplicationDtos = CrmResponseSerializer.Deserialize<List<LoanApplicationDto>>(response.invln_loanapplications)
                                  ?? throw new NotFoundException("Applications list", userAccount.ToString());

        return loanApplicationDtos.Select(x =>
                new UserLoanApplication(
                    LoanApplicationId.From(x.loanApplicationId),
                    LoanApplicationName.CreateOrDefault(x.ApplicationName),
                    ApplicationStatusMapper.MapToPortalStatus(x.loanApplicationExternalStatus),
                    x.createdOn,
                    x.LastModificationOn,
                    x.lastModificationByName)).ToList();
    }

    public async Task Save(LoanApplicationEntity loanApplication, UserProfileDetails userDetails, CancellationToken cancellationToken)
    {
        var loanApplicationDto = new LoanApplicationDto
        {
            LoanApplicationContact = LoanApplicationMapper.MapToUserAccountDto(loanApplication.UserAccount, userDetails),
            fundingReason = FundingPurposeMapper.Map(loanApplication.FundingReason),
            ApplicationName = loanApplication.Name.Value,
            frontDoorProjectId = loanApplication.FrontDoorProjectId?.Value,
        };

        var loanApplicationSerialized = CrmResponseSerializer.Serialize(loanApplicationDto);
        var req = new invln_sendinvestmentloansdatatocrmRequest
        {
            invln_entityfieldsparameters = loanApplicationSerialized,
            invln_accountid = loanApplication.UserAccount.SelectedOrganisationId().ToString(),
            invln_contactexternalid = loanApplication.UserAccount.UserGlobalId.ToString(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        var response = (invln_sendinvestmentloansdatatocrmResponse)await _serviceClient.ExecuteAsync(req, cancellationToken);
        var newLoanApplicationId = LoanApplicationId.From(response.invln_loanapplicationid);
        loanApplication.SetId(newLoanApplicationId);
    }

    public async Task Submit(LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        await ChangeApplicationStatus(loanApplicationId, ApplicationStatus.ApplicationSubmitted, cancellationToken);
    }

    public async Task ChangeApplicationStatus(LoanApplicationId loanApplicationId, ApplicationStatus applicationStatus, CancellationToken cancellationToken)
    {
        var crmStatus = ApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_statusexternal = crmStatus,
        };

        await _serviceClient.ExecuteAsync(request, cancellationToken);
    }

    public async Task WithdrawSubmitted(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken)
    {
        await ChangeApplicationStatusWithChangeReason(loanApplicationId, ApplicationStatus.Withdrawn, withdrawReason.ToString(), cancellationToken);
    }

    public async Task WithdrawDraft(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken)
    {
        await ChangeApplicationStatusWithChangeReason(loanApplicationId, ApplicationStatus.NA, withdrawReason.ToString(), cancellationToken);
    }

    public async Task MoveToDraft(LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        await ChangeApplicationStatus(loanApplicationId, ApplicationStatus.Draft, cancellationToken);
    }

    public async Task<FileLocation> GetFilesLocationAsync(SupportingDocumentsParams fileParams, CancellationToken cancellationToken)
    {
        var basePath = await _fileRepository.GetBaseFilePath(fileParams.LoanApplicationId, cancellationToken);
        return new FileLocation(
            _documentSettings.ListTitle,
            _documentSettings.ListAlias,
            $"{basePath}{LoanApplicationConstants.SupportingDocumentsExternal}");
    }

    public async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }

    private async Task ChangeApplicationStatusWithChangeReason(LoanApplicationId loanApplicationId, ApplicationStatus applicationStatus, string changeReason, CancellationToken cancellationToken)
    {
        var crmStatus = ApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_statusexternal = crmStatus,
            invln_changereason = changeReason,
        };

        await _serviceClient.ExecuteAsync(request, cancellationToken);
    }
}
