using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository, ICanSubmitLoanApplication
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDateTimeProvider _dateTime;

    public LoanApplicationRepository(IOrganizationServiceAsync2 serviceClient, IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
    {
        _serviceClient = serviceClient;
        _httpContextAccessor = httpContextAccessor;
        _dateTime = dateTime;
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

        return new LoanApplicationEntity(id, userAccount, externalStatus, FundingPurposeMapper.Map(loanApplicationDto.fundingReason), null, loanApplicationDto.LastModificationOn)
        {
            LegacyModel = LoanApplicationMapper.Map(loanApplicationDto, _dateTime.Now),
        };
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
                x.name,
                ApplicationStatusMapper.MapToPortalStatus(x.loanApplicationExternalStatus),
                x.createdOn,
                x.LastModificationOn)).ToList();
    }

    public async Task Save(LoanApplicationEntity loanApplication, UserDetails userDetails, CancellationToken cancellationToken)
    {
        var siteDetailsDtos = new List<SiteDetailsDto>();
        foreach (var site in loanApplication.ApplicationProjects.Projects)
        {
            var siteDetail = new SiteDetailsDto()
            {
                Name = site.Name?.Value,
                siteName = site.Name?.Value,
            };
            siteDetailsDtos.Add(siteDetail);
        }

        var loanApplicationDto = new LoanApplicationDto()
        {
            LoanApplicationContact = LoanApplicationMapper.MapToUserAccountDto(loanApplication.UserAccount, userDetails),
            fundingReason = FundingPurposeMapper.Map(loanApplication.FundingReason),
            siteDetailsList = siteDetailsDtos,
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
        LegacySave(loanApplication.LegacyModel);
    }

    public async Task Submit(LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var crmSubmitStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.ApplicationSubmitted);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_statusexternal = crmSubmitStatus,
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

    public void LegacySave(LoanApplicationViewModel legacyModel)
    {
        legacyModel.Timestamp = _dateTime.Now;

        _httpContextAccessor.HttpContext?.Session.Set(legacyModel.ID.ToString(), legacyModel);
    }
}
