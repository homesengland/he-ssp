using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.Xrm.Sdk;

namespace HE.InvestmentLoans.BusinessLogic.Application.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly IOrganizationService _serviceClient;

    public LoanApplicationRepository(IOrganizationService serviceClient)
    {
        _serviceClient = serviceClient;
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
            accountId = userAccount.AccountId,
            externalId = userAccount.UserGlobalId,
            contactEmailAdress = loanApplication.Account.EmailAddress,

            // COMPANY
            companyPurpose = loanApplication.Company.Purpose,
            existingCompany = loanApplication.Company.ExistingCompany,
            fundingReason = MapPurpose(loanApplication.Purpose),
            companyExperience = loanApplication.Company.HomesBuilt.TryParseNullableInt(),

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
        };

        req.Parameters.Add("invln_contactexternalid", userAccount.UserGlobalId);
        req.Parameters.Add("invln_accountid", userAccount.AccountId.ToString());

        _serviceClient.Execute(req);
    }

    public string MapPurpose(FundingPurpose? fundingPurpose)
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
