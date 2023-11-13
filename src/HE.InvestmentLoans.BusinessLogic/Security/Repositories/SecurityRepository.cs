using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.Security.Mappers;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.Security.Repositories;

internal class SecurityRepository : ISecurityRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public SecurityRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task<SecurityEntity> GetAsync(
                                            LoanApplicationId applicationId,
                                            UserAccount userAccount,
                                            SecurityFieldsSet securityFieldsSet,
                                            CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = SecurityCrmFieldNameMapper.Map(securityFieldsSet);
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = applicationId.ToString(),
            invln_fieldstoretrieve = fieldsToRetrieve,
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                       ?? throw new NotFoundException(nameof(SecurityEntity), applicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(SecurityEntity), applicationId.ToString());

        var debenture = loanApplicationDto.outstandingLegalChargesOrDebt.HasValue
            ? new Debenture(
                loanApplicationDto.debentureHolder,
                loanApplicationDto.outstandingLegalChargesOrDebt.Value)
            : null;

        var directLoans = loanApplicationDto.directorLoans.HasValue
            ? new DirectorLoans(
                loanApplicationDto.directorLoans.Value)
            : null;

        var directLoansSubordinate = loanApplicationDto.confirmationDirectorLoansCanBeSubordinated.HasValue
            ? new DirectorLoansSubordinate(
                loanApplicationDto.confirmationDirectorLoansCanBeSubordinated.Value,
                loanApplicationDto.reasonForDirectorLoanNotSubordinated)
            : null;

        return new SecurityEntity(
            applicationId,
            debenture!,
            directLoans!,
            directLoansSubordinate!,
            SectionStatusMapper.Map(loanApplicationDto.SecurityDetailsCompletionStatus),
            ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus));
    }

    public async Task SaveAsync(SecurityEntity entity, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var loanApplicationDto = new LoanApplicationDto
        {
            debentureHolder = entity.Debenture?.Holder,
            outstandingLegalChargesOrDebt = entity.Debenture?.Exists,
            directorLoans = entity.DirectorLoans?.Exists,
            confirmationDirectorLoansCanBeSubordinated = entity.DirectorLoansSubordinate?.CanBeSubordinated,
            reasonForDirectorLoanNotSubordinated = entity.DirectorLoansSubordinate?.ReasonWhyCannotBeSubordinated,
            SecurityDetailsCompletionStatus = SectionStatusMapper.Map(entity.Status),
        };

        var loanApplicationSerialized = CrmResponseSerializer.Serialize(loanApplicationDto);
        var req = new invln_updatesingleloanapplicationRequest
        {
            invln_loanapplication = loanApplicationSerialized,
            invln_loanapplicationid = entity.LoanApplicationId.Value.ToString(),
            invln_accountid = userAccount.AccountId.ToString(),
            invln_contactexternalid = userAccount.UserGlobalId.ToString(),
            invln_fieldstoupdate = string.Join(',', SecurityCrmFieldNameMapper.Map(SecurityFieldsSet.SaveAllFields)),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }
}
