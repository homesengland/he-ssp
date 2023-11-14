using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Funding.Mappers;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
public class FundingRepository : IFundingRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public FundingRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task<FundingEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, FundingFieldsSet fundingFieldsSet, CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = FundingCrmFieldNameMapper.Map(fundingFieldsSet);
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_fieldstoretrieve = fieldsToRetrieve,
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
            invln_accountid = userAccount.AccountId.ToString(),
            invln_contactexternalid = userAccount.UserGlobalId.ToString(),
            invln_fieldstoupdate = FundingCrmFieldNameMapper.Map(FundingFieldsSet.SaveAllFields),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }
}
