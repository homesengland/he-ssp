using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.DocumentService.Models;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Mappers;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;

public class CompanyStructureRepository : ICompanyStructureRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IFileApplicationRepository _fileRepository;

    private readonly ILoansDocumentSettings _documentSettings;

    private readonly IEventDispatcher _eventDispatcher;

    public CompanyStructureRepository(
        IOrganizationServiceAsync2 serviceClient,
        IFileApplicationRepository fileRepository,
        ILoansDocumentSettings documentSettings,
        IEventDispatcher eventDispatcher)
    {
        _serviceClient = serviceClient;
        _fileRepository = fileRepository;
        _documentSettings = documentSettings;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<CompanyStructureEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CompanyStructureFieldsSet companyStructureFieldsSet, CancellationToken cancellationToken)
    {
        var fieldsToRetrieve = CompanyStructureCrmFieldNameMapper.Map(companyStructureFieldsSet);

        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_fieldstoretrieve = fieldsToRetrieve,
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                       ?? throw new NotFoundException(nameof(CompanyStructureEntity), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(CompanyStructureEntity), loanApplicationId.ToString());

        return new CompanyStructureEntity(
            loanApplicationId,
            CompanyStructureMapper.MapCompanyPurpose(loanApplicationDto.companyPurpose),
            CompanyStructureMapper.MapMoreInformation(loanApplicationDto.existingCompany),
            CompanyStructureMapper.MapHomesBuild(loanApplicationDto.companyExperience),
            SectionStatusMapper.Map(loanApplicationDto.CompanyStructureAndExperienceCompletionStatus),
            ApplicationStatusMapper.MapToPortalStatus(loanApplicationDto.loanApplicationExternalStatus),
            _eventDispatcher);
    }

    public async Task<FileLocation> GetFilesLocationAsync(LoanApplicationId fileParams, CancellationToken cancellationToken)
    {
        var basePath = await _fileRepository.GetBaseFilePath(fileParams, cancellationToken);
        return new FileLocation(
            _documentSettings.ListTitle,
            _documentSettings.ListAlias,
            $"{basePath}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}");
    }

    public async Task SaveAsync(CompanyStructureEntity companyStructure, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var loanApplicationDto = new LoanApplicationDto
        {
            companyPurpose = CompanyStructureMapper.MapCompanyPurpose(companyStructure.Purpose),
            existingCompany = companyStructure.MoreInformation?.Information,
            companyExperience = companyStructure.HomesBuilt?.Value,
            CompanyStructureAndExperienceCompletionStatus = SectionStatusMapper.Map(companyStructure.Status),
        };

        var loanApplicationSerialized = JsonSerializer.Serialize(loanApplicationDto);
        var req = new invln_updatesingleloanapplicationRequest
        {
            invln_loanapplication = loanApplicationSerialized,
            invln_loanapplicationid = companyStructure.LoanApplicationId.Value.ToString(),
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_contactexternalid = userAccount.UserGlobalId.ToString(),
            invln_fieldstoupdate = CompanyStructureCrmFieldNameMapper.Map(CompanyStructureFieldsSet.SaveAllFields),
        };

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }
}
