using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM.Extensions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.Loans.BusinessLogic.PrefillData.Entities;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.FeatureManagement;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;

public class LoanPrefillDataRepository : ILoanPrefillDataRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IFeatureManager _featureManager;

    private readonly IPrefillDataRepository _prefillDataRepository;

    public LoanPrefillDataRepository(IPrefillDataRepository prefillDataRepository, IOrganizationServiceAsync2 serviceClient, IFeatureManager featureManager)
    {
        _prefillDataRepository = prefillDataRepository;
        _serviceClient = serviceClient;
        _featureManager = featureManager;
    }

    public async Task<LoanPrefillData?> GetLoanApplicationPrefillData(LoanApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var frontDoorProjectId = await GetFrontDoorProjectId(applicationId, userAccount, cancellationToken);
        if (frontDoorProjectId.IsNotProvided())
        {
            return null;
        }

        var prefillData = await _prefillDataRepository.GetProjectPrefillData(frontDoorProjectId!, userAccount, cancellationToken);

        return new LoanPrefillData(prefillData.Name);
    }

    private static string FormatFields(params string[] fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }

    private async Task<FrontDoorProjectId?> GetFrontDoorProjectId(
        LoanApplicationId loanApplicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_fieldstoretrieve = FormatFields(
                nameof(invln_Loanapplication.invln_LoanapplicationId),
                nameof(invln_Loanapplication.invln_loanapplication_FDProjectId_invln_frontdo)),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse;
        if (response.IsNotProvided())
        {
            return null;
        }

        var dto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response!.invln_loanapplication)?.FirstOrDefault();
        if (dto.IsNotProvided() || string.IsNullOrWhiteSpace(dto?.frontDoorProjectId))
        {
            return null;
        }

        return new FrontDoorProjectId(dto.frontDoorProjectId);
    }
}
