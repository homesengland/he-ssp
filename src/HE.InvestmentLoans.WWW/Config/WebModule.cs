using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Config;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Infrastructure;
using HE.InvestmentLoans.CRM.Extensions;
using HE.InvestmentLoans.WWW.Models;
using HE.InvestmentLoans.WWW.Notifications;
using HE.Investments.Common.Config;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Organisation.Config;

namespace HE.InvestmentLoans.WWW.Config;

public static class WebModule
{
    public static void AddWebModule(this IServiceCollection serviceCollections)
    {
        serviceCollections.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoanApplicationViewModel).Assembly));
        serviceCollections.AddScoped<NonceModel>();
        serviceCollections.AddCrmConnection();
        serviceCollections.AddBusinessLogic();
        serviceCollections.AddValidatorsFromAssemblyContaining<LoanPurposeModel>();

        serviceCollections.AddNotifications(typeof(LoanApplicationHasBeenResubmittedDisplayNotificationFactory).Assembly);
        serviceCollections.AddOrganizationsModule();
        serviceCollections.AddEventInfrastructure();
        serviceCollections.AddHttpUserContext();
    }
}
