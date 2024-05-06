using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.BusinessLogic.Funding.Mappers;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Loans.BusinessLogic.Funding.Repositories;

public class FundingRepository : IFundingRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IEventDispatcher _eventDispatcher;

    public FundingRepository(IOrganizationServiceAsync2 serviceClient, IEventDispatcher eventDispatcher)
    {
        _serviceClient = serviceClient;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<FundingEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, FundingFieldsSet fundingFieldsSet, CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = FundingCrmFieldNameMapper.Map(fundingFieldsSet);
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToGuidAsString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_fieldstoretrieve = fieldsToRetrieve,
            invln_usehetables = "true",
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                       ?? throw new NotFoundException(nameof(FundingEntity), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(FundingEntity), loanApplicationId.ToString());

        return new FundingEntity(
            loanApplicationId,
            FundingEntityMapper.MapGrossDevelopmentValue(loanApplicationDto.projectGdv),
            FundingEntityMapper.MapEstimatedTotalCosts(loanApplicationDto.projectEstimatedTotalCost),
            FundingEntityMapper.MapAbnormalCosts(loanApplicationDto.projectAbnormalCosts, loanApplicationDto.projectAbnormalCostsInformation),
            FundingEntityMapper.MapPrivateSectorFunding(loanApplicationDto.privateSectorApproach, loanApplicationDto.privateSectorApproachInformation),
            FundingEntityMapper.MapRepaymentSystem(loanApplicationDto.refinanceRepayment, loanApplicationDto.refinanceRepaymentDetails),
            FundingEntityMapper.MapAdditionalProjects(loanApplicationDto.additionalProjects),
            SectionStatusMapper.Map(loanApplicationDto.FundingDetailsCompletionStatus),
            ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus));
    }

    public async Task SaveAsync(FundingEntity funding, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var loanApplicationDto = new LoanApplicationDto
        {
            projectGdv = FundingEntityMapper.MapGrossDevelopmentValue(funding.GrossDevelopmentValue),
            projectEstimatedTotalCost = FundingEntityMapper.MapEstimatedTotalCosts(funding.EstimatedTotalCosts),
            projectAbnormalCosts = funding.AbnormalCosts?.IsAnyAbnormalCost,
            projectAbnormalCostsInformation = funding.AbnormalCosts?.AbnormalCostsAdditionalInformation,
            privateSectorApproach = funding.PrivateSectorFunding?.IsApplied,
            privateSectorApproachInformation = FundingEntityMapper.MapPrivateSectorFundingAdditionalInformation(funding.PrivateSectorFunding),
            additionalProjects = funding.AdditionalProjects?.IsThereAnyAdditionalProject,
            refinanceRepayment = FundingEntityMapper.MapRepaymentSystem(funding.RepaymentSystem),
            refinanceRepaymentDetails = funding.RepaymentSystem?.Refinance?.AdditionalInformation,
            FundingDetailsCompletionStatus = SectionStatusMapper.Map(funding.Status),
        };

        var loanApplicationSerialized = JsonSerializer.Serialize(loanApplicationDto);
        var req = new invln_updatesingleloanapplicationRequest
        {
            invln_loanapplication = loanApplicationSerialized,
            invln_loanapplicationid = funding.LoanApplicationId.Value.ToString(),
            invln_accountid = userAccount.SelectedOrganisationId().ToGuidAsString(),
            invln_contactexternalid = userAccount.UserGlobalId.ToString(),
            invln_fieldstoupdate = FundingCrmFieldNameMapper.Map(FundingFieldsSet.SaveAllFields),
            invln_usehetables = "true",
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
        await _eventDispatcher.Publish(
            new LoanApplicationHasBeenChangedEvent(funding.LoanApplicationId, funding.LoanApplicationStatus),
            cancellationToken);
    }
}
