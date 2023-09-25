using System.Security.Policy;
using System.Threading;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;

public class ApplicationProjectsRepository : IApplicationProjectsRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDateTimeProvider _dateTime;

    private readonly IOrganizationServiceAsync2 _serviceClient;

    public ApplicationProjectsRepository(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime, IOrganizationServiceAsync2 serviceClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _dateTime = dateTime;
        _serviceClient = serviceClient;
    }

    public ApplicationProjects GetAll(LoanApplicationId loanApplicationId, UserAccount userAccount)
    {
        var loanApplication = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationEntity>(loanApplicationId.ToString())
            ?? throw new NotFoundException(nameof(LoanApplicationEntity).ToString(), loanApplicationId.ToString());

        return loanApplication.ApplicationProjects;
    }

    public LoanApplicationViewModel LegacyDeleteProject(Guid loanApplicationId, Guid projectId)
    {
        var loanApplicationSessionModel = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(loanApplicationId.ToString())!;

        var projectToDelete = loanApplicationSessionModel.Sites.FirstOrDefault(p => p.Id == projectId) ?? throw new NotFoundException(nameof(SiteViewModel).ToString(), projectId);

        loanApplicationSessionModel!.SetTimestamp(_dateTime.Now);
        loanApplicationSessionModel!.Sites.Remove(projectToDelete);

        _httpContextAccessor.HttpContext?.Session.Set(loanApplicationId.ToString(), loanApplicationSessionModel);

        return loanApplicationSessionModel;
    }

    public async Task<ApplicationProjects> GetById(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                      ?? throw new NotFoundException(nameof(Project), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(ApplicationProjects), loanApplicationId.ToString());

        var projectsFromCrm = loanApplicationDto.siteDetailsList.Select(
            projectFromCrm => new Project
            {
                Id = ProjectId.From(projectFromCrm.siteDetailsId),
                NameLegacy = projectFromCrm.siteName,
                ManyHomes = projectFromCrm.numberOfHomes,
                TypeHomes = projectFromCrm.typeOfHomes,
                TypeHomesOther = projectFromCrm.otherTypeOfHomes,
                Type = projectFromCrm.typeOfSite,
                PlanningRef = projectFromCrm.haveAPlanningReferenceNumber,
                PlanningRefEnter = projectFromCrm.planningReferenceNumber,
                LocationCoordinates = projectFromCrm.siteCoordinates,
                Ownership = projectFromCrm.siteOwnership,
                LocationLandRegistry = projectFromCrm.landRegistryTitleNumber,
                PurchaseDate = projectFromCrm.dateOfPurchase,
                Cost = projectFromCrm.siteCost,
                Value = projectFromCrm.currentValue,
                Source = projectFromCrm.valuationSource,
                GrantFunding = projectFromCrm.publicSectorFunding,
                GrantFundingAmount = projectFromCrm.howMuch,
                GrantFundingName = projectFromCrm.nameOfGrantFund,
                GrantFundingPurpose = projectFromCrm.reason,
                ChargesDebt = projectFromCrm.existingLegalCharges,
                ChargesDebtInfo = projectFromCrm.existingLegalChargesInformation,
                AffordableHomes = projectFromCrm.numberOfAffordableHomes,
            });

        return new ApplicationProjects(loanApplicationId, projectsFromCrm);
    }

    public async Task<Project> GetById(LoanApplicationId loanApplicationId, ProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getsingleloanapplicationforaccountandcontactRequest
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_loanapplicationid = loanApplicationId.ToString(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getsingleloanapplicationforaccountandcontactResponse
                      ?? throw new NotFoundException(nameof(Project), loanApplicationId.ToString());

        var loanApplicationDto = CrmResponseSerializer.Deserialize<IList<LoanApplicationDto>>(response.invln_loanapplication)?.FirstOrDefault()
                                 ?? throw new NotFoundException(nameof(Project), projectId.ToString());

        var projectFromCrm = loanApplicationDto.siteDetailsList.FirstOrDefault(c => c.siteDetailsId == projectId.Value.ToString())
                                ?? throw new NotFoundException(nameof(Project), projectId.ToString());

        return new Project
        {
            Id = ProjectId.From(projectFromCrm.siteDetailsId),
            NameLegacy = projectFromCrm.siteName,
            ManyHomes = projectFromCrm.numberOfHomes,
            TypeHomes = projectFromCrm.typeOfHomes,
            TypeHomesOther = projectFromCrm.otherTypeOfHomes,
            Type = projectFromCrm.typeOfSite,
            PlanningRef = projectFromCrm.haveAPlanningReferenceNumber,
            PlanningRefEnter = projectFromCrm.planningReferenceNumber,
            LocationCoordinates = projectFromCrm.siteCoordinates,
            Ownership = projectFromCrm.siteOwnership,
            LocationLandRegistry = projectFromCrm.landRegistryTitleNumber,
            PurchaseDate = projectFromCrm.dateOfPurchase,
            Cost = projectFromCrm.siteCost,
            Value = projectFromCrm.currentValue,
            Source = projectFromCrm.valuationSource,
            GrantFunding = projectFromCrm.publicSectorFunding,
            GrantFundingAmount = projectFromCrm.howMuch,
            GrantFundingName = projectFromCrm.nameOfGrantFund,
            GrantFundingPurpose = projectFromCrm.reason,
            ChargesDebt = projectFromCrm.existingLegalCharges,
            ChargesDebtInfo = projectFromCrm.existingLegalChargesInformation,
            AffordableHomes = projectFromCrm.numberOfAffordableHomes,
        };
    }

    public async Task SaveAsync(ApplicationProjects applicationProjects, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var siteDetailsDtos = new List<SiteDetailsDto>();
        foreach (var site in applicationProjects.Projects)
        {
            var siteDetail = new SiteDetailsDto()
            {
                Name = site.NameLegacy,
                siteName = site.NameLegacy,
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

        var loanApplicationDto = new LoanApplicationDto
        {
            siteDetailsList = siteDetailsDtos,
        };

        var loanApplicationSerialized = CrmResponseSerializer.Serialize(loanApplicationDto);

        //var req = new invln_createsinglesitedetailRequest

        //var req = new invln_updatesingleloanapplicationRequest
        //{
        //    invln_loanapplication = loanApplicationSerialized,
        //    invln_loanapplicationid = applicationProjects.LoanApplicationId.Value.ToString(),
        //    invln_accountid = userAccount.AccountId.ToString(),
        //    invln_contactexternalid = userAccount.UserGlobalId.ToString(),
        //    invln_fieldstoupdate = string.Join(",", CrmSecurityFieldNames()),
        //};

        await _serviceClient.ExecuteAsync(req, cancellationToken);
    }

    private IEnumerable<string> CrmSecurityFieldNames()
    {
        yield return nameof(invln_Loanapplication.invln_projectestimatedtotalcost_Base).ToLowerInvariant();
        yield return nameof(invln_Loanapplication.invln_projectgdv_Base).ToLowerInvariant();
        yield return nameof(invln_Loanapplication.invln_Projectabnormalcosts).ToLowerInvariant();
        yield return nameof(invln_Loanapplication.invln_Projectabnormalcostsinformation).ToLowerInvariant();
        yield return nameof(invln_Loanapplication.invln_Projectestimatedtotalcost).ToLowerInvariant();
        yield return nameof(invln_Loanapplication.invln_ProjectGDV).ToLowerInvariant();
        yield return nameof(invln_Loanapplication.invln_ProjectName).ToLowerInvariant();
        yield return nameof(invln_Loanapplication.invln_Additionalprojects).ToLowerInvariant();
    }
}
