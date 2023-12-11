using FluentValidation;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Config;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.ViewModel;
using HE.Investments.Loans.Common.Infrastructure;
using HE.Investments.Loans.WWW.Models;
using HE.Investments.Loans.WWW.Notifications;
using HE.Investments.Loans.WWW.Routing;
using HE.Investments.Organisation.Config;

namespace HE.Investments.Loans.WWW.Config;

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
        serviceCollections.AddScoped<IAccountRoutes, LoansAccountRoutes>();
        serviceCollections.AddSingleton<IErrorViewPaths, LoansErrorViewPaths>();
    }
}
