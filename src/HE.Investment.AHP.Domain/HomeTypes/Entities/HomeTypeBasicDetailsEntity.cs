using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public record HomeTypeBasicDetailsEntity(HomeTypeId Id, HomeTypeName? Name, HousingType HousingType, int NumberOfHomes);
