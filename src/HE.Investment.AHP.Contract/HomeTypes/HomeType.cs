using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeType(HomeTypeId HomeTypeId, string HomeTypeName, HousingType HousingType, Tenure Tenure, HomeTypeConditionals Conditionals, bool IsReadOnly);
