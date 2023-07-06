using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Enums;
using HE.InvestmentLoans.BusinessLogic.Extensions;
using HE.InvestmentLoans.BusinessLogic.Repositories;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Authorization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands;

public class SendToCrm : IRequest<bool>
{
    public LoanApplicationViewModel Model
    {
        get;
        set;
    }

    public class Handler : IRequestHandler<SendToCrm, bool>
    {
        private readonly ILoanApplicationRepository _loanApplicationRepository;

        private readonly ILoanUserContext _loanUserContext;

        public Handler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext)
        {
            _loanApplicationRepository = loanApplicationRepository;
            _loanUserContext = loanUserContext;
        }

        public async Task<bool> Handle(SendToCrm request, CancellationToken cancellationToken)
        {
            List<SiteDetailsDto> siteDetailsDtos = new List<SiteDetailsDto>();
            foreach (var site in request.Model.Sites)
            {
                var siteDetail = new SiteDetailsDto()
                {
                    siteName = site.Name, // Name
                    numberOfHomes = site.ManyHomes, //site/ManyHomes
                    typeOfHomes = site.TypeHomes, //site/TypeHomes
                    otherTypeOfHomes = site.TypeHomesOther, //site/TypeHomes
                    typeOfSite = site.Type, //site/Type
                    haveAPlanningReferenceNumber = site.PlanningRef, //site/PlanningRef
                    planningReferenceNumber = site.PlanningRefEnter, //site/PlanningRef
                    siteCoordinates = site.LocationCoordinates, //site/Location
                    siteOwnership = site.Ownership, //site/Ownership
                    landRegistryTitleNumber = site.LocationLandRegistry, //site/Location
                    dateOfPurchase = site.PurchaseDate, //site.PurchaseDate - probably not correct field, //site/Additional
                    siteCost = site.Cost, //site/Additional
                    currentValue = site.Value, //site/Additional
                    valuationSource = site.Source, //site/Additional
                    publicSectorFunding = site.GrantFunding, //site/GrantFunding
                    whoProvided = site.GrantFundingSource, //site/GrantFundingMore
                    howMuch = site.GrantFundingAmount, //site/GrantFundingMore
                    nameOfGrantFund = site.GrantFundingName, //site/GrantFundingMore
                    reason = site.GrantFundingPurpose, //site/GrantFundingMore
                    existingLegalCharges = site.ChargesDebt, //site/ChargesDebt
                    existingLegalChargesInformation = site.ChargesDebtInfo, //site/ChargesDebt
                    numberOfAffordableHomes = site.AffordableHomes, //site/AffordableHomes
                };
                siteDetailsDtos.Add(siteDetail);
            }

            LoanApplicationDto loanApplicationDto = new LoanApplicationDto()
            {
                accountId = await _loanUserContext.GetSelectedAccountId(),
                externalId = _loanUserContext.UserGlobalId,
                name = request.Model.Account.RegisteredName,
                //sessionModel.Account.RegistrationNumber
                //sessionModel.Account.Address
                //sessionModel.Account.ContactName
                contactEmailAdress = request.Model.Account.EmailAddress,

                //COMPANY
                companyPurpose = request.Model.Company.Purpose,//Purpose
                existingCompany = request.Model.Company.ExistingCompany,//ExistingCompany
                fundingReason = MapPurpose(request.Model.Purpose),//LoanPurpose
                companyExperience = request.Model.Company.HomesBuilt.TryParseNullableInt(),//HomesBuilt
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

            _loanApplicationRepository.Save(loanApplicationDto);

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
