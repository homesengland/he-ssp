using FluentValidation;
using HE.Investments.Common;
using HE.Investments.Common.Config;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.WWW.Config;
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
    public static void AddWebModule(this IServiceCollection services)
    {
        services.AddAppConfiguration<IMvcAppConfig, MvcAppConfig>();
        services.AddAppConfiguration<FrontDoorConfig>("FrontDoorService");
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoanApplicationViewModel).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainValidationHandler<,,>).Assembly));
        services.AddScoped<NonceModel>();
        services.AddCrmConnection();
        services.AddBusinessLogic();
        services.AddValidatorsFromAssemblyContaining<LoanPurposeModel>();
        services.AddNotificationPublisher(ApplicationType.Loans);
        services.AddNotificationConsumer(ApplicationType.Loans, typeof(LoanApplicationHasBeenResubmittedDisplayNotificationFactory).Assembly);
        services.AddOrganisationCrmModule();
        services.AddEventInfrastructure();
        services.AddHttpUserContext();
        services.AddSingleton<IErrorViewPaths, LoansErrorViewPaths>();
    }
}
