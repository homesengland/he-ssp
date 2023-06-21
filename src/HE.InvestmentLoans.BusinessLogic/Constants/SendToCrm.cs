﻿using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Enums;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Rest;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands
{
    public class SendToCrm : IRequest<bool>
    {
        public LoanApplicationViewModel Model
        {
            get;
            set;
        }

        public class Handler : IRequestHandler<SendToCrm, bool>
        {
            private ServiceClient _serviceClient;

            public Handler(ServiceClient serviceClient)
            {
                _serviceClient = serviceClient;
            }

            public async Task<bool> Handle(SendToCrm request, CancellationToken cancellationToken)
            {
                List<SiteDetailsDto> siteDetailsDtos = new List<SiteDetailsDto>();
                foreach (var site in request.Model.Sites)
                {
                    var siteDetail = new SiteDetailsDto()
                    {
                        siteName = site.Name, // Name
                        numberOfHomes = site.ManyHomes, //site/ManyHomes ///
                        typeOfHomes = site.TypeHomes, //
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
                        whoProvided = site.GrantFundingSource,
                        howMuch = site.GrantFundingAmount,
                        nameOfGrantFund = site.GrantFundingName,
                        reason = site.GrantFundingPurpose,
                        existingLegalCharges = site.ChargesDebt,
                        existingLegalChargesInformation = site.ChargesDebtInfo,
                        numberOfAffordableHomes = site.AffordableHomes,
                    };
                    siteDetailsDtos.Add(siteDetail);
                }

                LoanApplicationDto loanApplicationDto = new LoanApplicationDto()
                {
                    accountId = request.Model.ID,
                    name = request.Model.Account.RegisteredName,
                    //sessionModel.Account.RegistrationNumber
                    //sessionModel.Account.Address
                    //sessionModel.Account.ContactName
                    contactEmailAdress = request.Model.Account.EmailAddress,

                    //COMPANY
                    companyPurpose = request.Model.Company.Purpose,//Purpose
                    existingCompany = request.Model.Company.ExistingCompany,//ExistingCompany
                    companyExperience = request.Model.Company.HomesBuilt,//HomesBuilt
                    fundingReason = MapPurpose(request.Model.Purpose),//LoanPurpose
                                                                      //Company.CompanyInfoFile

                    //FUNDING
                    projectGdv = request.Model.Funding.GrossDevelopmentValue, //GDV
                    projectEstimatedTotalCost = request.Model.Funding.TotalCosts, //TotalCosts
                    projectAbnormalCosts = request.Model.Funding.AbnormalCosts, //AbnormalCosts
                    projectAbnormalCostsInformation = request.Model.Funding.AbnormalCostsInfo, //AbnormalCosts
                    privateSectorApproach = request.Model.Funding.PrivateSectorFunding, //PrivateSectorFunding
                    privateSectorApproachInformation = request.Model.Funding.PrivateSectorFundingResult, //PrivateSectorFunding//
                    additionalProjects = request.Model.Funding.AdditionalProjects,//AdditionalProjects
                    refinanceRepayment = request.Model.Funding.Refinance, //Refinance
                    refinanceRepaymentDetails = request.Model.Funding.RefinanceInfo, //Refinance//
                                                                                     //Complete

                    //SECURITY
                    outstandingLegalChargesOrDebt = request.Model.Security.ChargesDebtCompany, //ChargesDebtCompany
                    debentureHolder = request.Model.Security.ChargesDebtCompanyInfo, //ChargesDebtCompany
                    directorLoans = request.Model.Security.DirLoans, //DirLoans
                    confirmationDirectorLoansCanBeSubordinated = request.Model.Security.DirLoansSub,//DirLoansSub
                    reasonForDirectorLoanNotSubordinated = request.Model.Security.DirLoansSubMore,//DirLoansSub

                    //SITEDETAILS
                    siteDetailsList = siteDetailsDtos,
                };

                string loanApplicationSerialized = JsonSerializer.Serialize<LoanApplicationDto>(loanApplicationDto);
                var req = new OrganizationRequest("invln_sendinvestmentloansdatatocrm")  //Name of Custom API
                {
                    ["invln_entityfieldsparameters"] = loanApplicationSerialized  //Input Parameter
                };

                var resp = _serviceClient.Execute(req);

                return true;
            }

            public string MapPurpose(FundingPurpose? fundingPurpose)
            {
                switch (fundingPurpose)
                {
                    case FundingPurpose.BuildingNewHomes:
                        return "Buildingnewhomes";
                    case FundingPurpose.BuildingInfrastructure:
                        return "Buildinginfrastructureonly";
                    case FundingPurpose.Other:
                        return "Other";
                    default:
                        return String.Empty;
                }
            }
        }
    }
}
