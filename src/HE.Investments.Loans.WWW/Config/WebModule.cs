using FluentValidation;
using HE.Investments.Common;
using HE.Investments.Common.Config;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.ViewModel;
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
        serviceCollections.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainValidationHandler<,,>).Assembly));
        serviceCollections.AddScoped<NonceModel>();
        serviceCollections.AddCrmConnection();
        serviceCollections.AddBusinessLogic();
        serviceCollections.AddValidatorsFromAssemblyContaining<LoanPurposeModel>();
        serviceCollections.AddNotificationPublisher(ApplicationType.Loans);
        serviceCollections.AddNotificationConsumer(ApplicationType.Loans, typeof(LoanApplicationHasBeenResubmittedDisplayNotificationFactory).Assembly);
        serviceCollections.AddOrganizationsModule();
        serviceCollections.AddEventInfrastructure();
        serviceCollections.AddHttpUserContext();
        serviceCollections.AddSingleton<IErrorViewPaths, LoansErrorViewPaths>();
        serviceCollections.AddSingleton(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:FrontDoorService").Get<FrontDoorConfig>());
    }
}
