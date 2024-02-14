using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.Mmc;

public record ProvideModernMethodsConstructionCategoriesCommand(
        SiteId SiteId,
        IList<ModernMethodsConstructionCategoriesType> ModernMethodsConstructionCategories)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
