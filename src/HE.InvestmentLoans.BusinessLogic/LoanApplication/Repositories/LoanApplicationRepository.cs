using System.Runtime.CompilerServices;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Exceptions;
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

        return new LoanApplicationEntity(id, userAccount, externalStatus, loanApplicationDto.LastModificationOn)
        {
            LegacyModel = LoanApplicationMapper.Map(loanApplicationDto),
        };
    }

    public async Task<UserLoanApplication> GetLoanApplicationSubmit(LoanApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
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

        return new UserLoanApplication(id, loanApplicationDto.name, externalStatus, null);
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
                x.LastModificationOn)).ToList();
    }

    public async Task Save(LoanApplicationViewModel loanApplication, UserAccount userAccount)
    {
        var siteDetailsDtos = new List<SiteDetailsDto>();
        foreach (var site in loanApplication.Sites)
        {
            var siteDetail = new SiteDetailsDto()
            {
                Name = site.Name,
                siteName = site.Name,
                numberOfHomes = site.ManyHomes,
                typeOfHomes = site.TypeHomes,
                otherTypeOfHomes = site.TypeHomesOther,
                typeOfSite = site.Type,
                haveAPlanningReferenceNumber = site.PlanningRef!.MapToBool(),
                planningReferenceNumber = site.PlanningRefEnter,
                siteCoordinates = site.LocationCoordinates,
                siteOwnership = site.Ownership!.MapToBool(),
                landRegistryTitleNumber = site.LocationLandRegistry,
                dateOfPurchase = site.PurchaseDate,
                siteCost = site.Cost,
                currentValue = site.Value,
                valuationSource = site.Source,
                publicSectorFunding = site.GrantFunding,
                howMuch = site.GrantFundingAmount,
                nameOfGrantFund = site.GrantFundingName,
                reason = site.GrantFundingPurpose,
                existingLegalCharges = site.ChargesDebt!.MapToBool(),
                existingLegalChargesInformation = site.ChargesDebtInfo,
                numberOfAffordableHomes = site.AffordableHomes,
            };
            siteDetailsDtos.Add(siteDetail);
        }

        var loanApplicationDto = new LoanApplicationDto()
        {
            loanApplicationId = loanApplication.ID.ToString(),
            name = loanApplication.Account.RegisteredName,
            contactEmailAdress = loanApplication.Account.EmailAddress,
            fundingReason = FundingPurposeMapper.Map(loanApplication.Purpose),

            // COMPANY
            companyPurpose = loanApplication.Company.Purpose!.MapToBool(),
            existingCompany = loanApplication.Company.OrganisationMoreInformation,
            companyExperience = loanApplication.Company.HomesBuilt?.TryParseNullableInt(),
            CompanyStructureAndExperienceCompletionStatus = SectionStatusMapper.Map(loanApplication.Company.State),

            // FUNDING
            projectGdv = loanApplication.Funding.GrossDevelopmentValue,
            projectEstimatedTotalCost = loanApplication.Funding.TotalCosts,
            projectAbnormalCosts = loanApplication.Funding.AbnormalCosts!.MapToBool(),
            projectAbnormalCostsInformation = loanApplication.Funding.AbnormalCostsInfo,
            privateSectorApproach = loanApplication.Funding.PrivateSectorFunding!.MapToBool(),
            privateSectorApproachInformation = loanApplication.Funding.PrivateSectorFundingResult,
            additionalProjects = loanApplication.Funding.AdditionalProjects!.MapToBool(),
            refinanceRepayment = loanApplication.Funding.Refinance,
            refinanceRepaymentDetails = loanApplication.Funding.RefinanceInfo,

            // SECURITY
            outstandingLegalChargesOrDebt = loanApplication.Security.ChargesDebtCompany!.MapToBool(),
            debentureHolder = loanApplication.Security.ChargesDebtCompanyInfo,
            directorLoans = loanApplication.Security.DirLoans!.MapToBool(),
            confirmationDirectorLoansCanBeSubordinated = loanApplication.Security.DirLoansSub!.MapToBool(),
            reasonForDirectorLoanNotSubordinated = loanApplication.Security.DirLoansSubMore,

            // SITEDETAILS
            siteDetailsList = siteDetailsDtos,
        };

        var loanApplicationSerialized = CrmResponseSerializer.Serialize(loanApplicationDto);
        var req = new invln_sendinvestmentloansdatatocrmRequest
        {
            invln_entityfieldsparameters = loanApplicationSerialized,
            invln_accountid = userAccount.AccountId.ToString(),
            invln_contactexternalid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplication.ID.ToString(),
        };

        await _serviceClient.ExecuteAsync(req);
    }

    public async Task Save(LoanApplicationEntity loanApplication, CancellationToken cancellationToken)
    {
        var loanApplicationDto = new LoanApplicationDto()
        {
            LoanApplicationContact = LoanApplicationMapper.MapToUserAccountDto(loanApplication.UserAccount),
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
        var crmSubmitStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.Submitted);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId.ToString(),
            invln_statusexternal = crmSubmitStatus,
        };

        await _serviceClient.ExecuteAsync(request, cancellationToken);
    }

    public void LegacySave(LoanApplicationViewModel legacyModel)
    {
        legacyModel.Timestamp = _dateTime.Now;

        _httpContextAccessor.HttpContext?.Session.Set(legacyModel.ID.ToString(), legacyModel);
    }
}
