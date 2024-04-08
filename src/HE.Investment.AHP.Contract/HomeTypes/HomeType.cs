using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeType(ApplicationDetails Application, HomeTypeId HomeTypeId, string HomeTypeName, HousingType HousingType, HomeTypeConditionals Conditionals);
