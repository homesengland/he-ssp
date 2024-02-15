using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands.Mmc;

public record ProvideModernMethodsConstruction3DSubcategoriesCommand(
        SiteId SiteId,
        IList<ModernMethodsConstruction3DSubcategoriesType> ModernMethodsConstruction3DSubcategories)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
