using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public LoanApplicationRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public LoanApplicationEntity Load(LoanApplicationId id, UserAccount userAccount)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId,
            invln_loanapplicationid = id.ToString(),
        };

        _serviceClient.ExecuteAsync(req);

        // TODO: It will be fullfilled with next PR.
        return new LoanApplicationEntity(id, new LoanApplicationViewModel());
    }

    public async Task<IList<UserLoanApplication>> LoadAllLoanApplications(UserAccount userAccount)
    {
        var req = new invln_getloanapplicationsforaccountandcontactRequest()
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId,
        };

        var response_async = await _serviceClient.ExecuteAsync(req);
        var response = response_async != null ? (invln_getloanapplicationsforaccountandcontactResponse)response_async : throw new NotFoundException("Applications list", userAccount.ToString());
        var loanApplicationDtos = JsonSerializer.Deserialize<List<LoanApplicationDto>>(response.invln_loanapplications) ?? throw new NotFoundException("Applications list", userAccount.ToString());

        return loanApplicationDtos.Select(x => new UserLoanApplication(LoanApplicationId.From(x.accountId), x.name, new ApplicationStatusMapper().MapToPortalStatus(x.loanApplicationStatus), x.LastModificationOn)).ToList();
    }

    public void Save(LoanApplicationViewModel loanApplication, UserAccount userAccount)
    {
        var siteDetailsDtos = new List<SiteDetailsDto>();
        foreach (var site in loanApplication.Sites)
        {
            var siteDetail = new SiteDetailsDto()
            {
                siteName = site.Name,
                numberOfHomes = site.ManyHomes,
                typeOfHomes = site.TypeHomes,
                otherTypeOfHomes = site.TypeHomesOther,
                typeOfSite = site.Type,
                haveAPlanningReferenceNumber = site.PlanningRef,
                planningReferenceNumber = site.PlanningRefEnter,
                siteCoordinates = site.LocationCoordinates,
                siteOwnership = site.Ownership,
                landRegistryTitleNumber = site.LocationLandRegistry,
                dateOfPurchase = site.PurchaseDate,
                siteCost = site.Cost,
                currentValue = site.Value,
                valuationSource = site.Source,
                publicSectorFunding = site.GrantFunding,
                howMuch = site.GrantFundingAmount,
                nameOfGrantFund = site.GrantFundingName,
                reason = site.GrantFundingPurpose,
                existingLegalCharges = site.ChargesDebt,
                existingLegalChargesInformation = site.ChargesDebtInfo,
                numberOfAffordableHomes = site.AffordableHomes,
            };
            siteDetailsDtos.Add(siteDetail);
        }

        var loanApplicationDto = new LoanApplicationDto()
        {
            name = loanApplication.Account.RegisteredName,
            contactEmailAdress = loanApplication.Account.EmailAddress,

            loanApplicationStatus = new ApplicationStatusMapper().MapToCrmStatus(ApplicationStatus.Submitted),

            // COMPANY
            companyPurpose = loanApplication.Company.Purpose,
            existingCompany = loanApplication.Company.ExistingCompany,
            fundingReason = MapPurpose(loanApplication.Purpose),
            companyExperience = loanApplication.Company.HomesBuilt?.TryParseNullableInt(),

            // FUNDING
            projectGdv = loanApplication.Funding.GrossDevelopmentValue,
            projectEstimatedTotalCost = loanApplication.Funding.TotalCosts,
            projectAbnormalCosts = loanApplication.Funding.AbnormalCosts,
            projectAbnormalCostsInformation = loanApplication.Funding.AbnormalCostsInfo,
            privateSectorApproach = loanApplication.Funding.PrivateSectorFunding,
            privateSectorApproachInformation = loanApplication.Funding.PrivateSectorFundingResult,
            additionalProjects = loanApplication.Funding.AdditionalProjects,
            refinanceRepayment = loanApplication.Funding.Refinance,
            refinanceRepaymentDetails = loanApplication.Funding.RefinanceInfo,

            // SECURITY
            outstandingLegalChargesOrDebt = loanApplication.Security.ChargesDebtCompany,
            debentureHolder = loanApplication.Security.ChargesDebtCompanyInfo,
            directorLoans = loanApplication.Security.DirLoans,
            confirmationDirectorLoansCanBeSubordinated = loanApplication.Security.DirLoansSub,
            reasonForDirectorLoanNotSubordinated = loanApplication.Security.DirLoansSubMore,

            // SITEDETAILS
            siteDetailsList = siteDetailsDtos,
        };

        var loanApplicationSerialized = JsonSerializer.Serialize(loanApplicationDto);
        var req = new invln_sendinvestmentloansdatatocrmRequest
        {
            invln_entityfieldsparameters = loanApplicationSerialized,
            invln_accountid = userAccount.AccountId.ToString(),
            invln_contactexternalid = userAccount.UserGlobalId,
        };

        _serviceClient.ExecuteAsync(req);
    }

    private string MapPurpose(FundingPurpose? fundingPurpose)
    {
        return fundingPurpose switch
        {
            FundingPurpose.BuildingNewHomes => "Buildingnewhomes",
            FundingPurpose.BuildingInfrastructure => "Buildinginfrastructureonly",
            FundingPurpose.Other => "Other",
            _ => string.Empty,
        };
    }
}
