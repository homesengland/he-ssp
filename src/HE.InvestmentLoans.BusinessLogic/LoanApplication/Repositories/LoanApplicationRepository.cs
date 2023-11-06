using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Infrastructure.Events;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository, ICanSubmitLoanApplication
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IDateTimeProvider _dateTime;

    private readonly IEventDispatcher _eventDispatcher;

    public LoanApplicationRepository(IOrganizationServiceAsync2 serviceClient, IDateTimeProvider dateTime, IEventDispatcher eventDispatcher)
    {
        _serviceClient = serviceClient;
        _dateTime = dateTime;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<bool> IsExist(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_fieldstoretrieve = nameof(invln_Loanapplication.invln_LoanapplicationId).ToLowerInvariant(),
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
            invln_organisationid = userAccount.AccountId?.ToString(),
        };

        var response = (invln_checkifloanapplicationwithgivennameexistsResponse)await _serviceClient.ExecuteAsync(req, cancellationToken);
        return response.invln_loanexists;
    }

    public async Task<LoanApplicationEntity> GetLoanApplication(LoanApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = id.ToString(),
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
            loanApplicationDto.name);
    }

    public async Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getloanapplicationsforaccountandcontactRequest()
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
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

    public async Task Save(LoanApplicationEntity loanApplication, UserDetails userDetails, CancellationToken cancellationToken)
    {
        var loanApplicationDto = new LoanApplicationDto()
        {
            LoanApplicationContact = LoanApplicationMapper.MapToUserAccountDto(loanApplication.UserAccount, userDetails),
            fundingReason = FundingPurposeMapper.Map(loanApplication.FundingReason),
            ApplicationName = loanApplication.Name.Value,
        };

        var loanApplicationSerialized = CrmResponseSerializer.Serialize(loanApplicationDto);
        var req = new invln_sendinvestmentloansdatatocrmRequest
        {
            invln_entityfieldsparameters = loanApplicationSerialized,
            invln_accountid = loanApplication.UserAccount.AccountId.ToString(),
            invln_contactexternalid = loanApplication.UserAccount.UserGlobalId.ToString(),
        };

        var response = (invln_sendinvestmentloansdatatocrmResponse)await _serviceClient.ExecuteAsync(req, cancellationToken);
        var newLoanApplicationId = LoanApplicationId.From(response.invln_loanapplicationid);
        loanApplication.SetId(newLoanApplicationId);
    }

    public async Task Submit(LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var crmSubmitStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.ApplicationSubmitted);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(), invln_statusexternal = crmSubmitStatus,
        };

        await _serviceClient.ExecuteAsync(request, cancellationToken);
    }

    public async Task WithdrawSubmitted(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken)
    {
        var crmWithdrawnStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.Withdrawn);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_statusexternal = crmWithdrawnStatus,
            invln_changereason = withdrawReason.ToString(),
        };

        await _serviceClient.ExecuteAsync(request, cancellationToken);
    }

    public async Task WithdrawDraft(LoanApplicationId loanApplicationId, WithdrawReason withdrawReason, CancellationToken cancellationToken)
    {
        var crmRemoveStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.NA);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_statusexternal = crmRemoveStatus,
            invln_changereason = withdrawReason.ToString(),
        };

        await _serviceClient.ExecuteAsync(request, cancellationToken);
    }

    public async Task MoveToDraft(LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var crmDraftStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.Draft);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_statusexternal = crmDraftStatus,
        };

        await _serviceClient.ExecuteAsync(request, cancellationToken);
    }

    public async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }
}
