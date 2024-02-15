using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.Mmc;

public record ProvideModernMethodsConstruction2DSubcategoriesCommand(
        SiteId SiteId,
        IList<ModernMethodsConstruction2DSubcategoriesType> ModernMethodsConstruction2DSubcategories)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
