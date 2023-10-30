using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypeEntity
{
    private readonly IDictionary<HomeTypeSegmentType, IHomeTypeSectionEntity> _segments;

    public HomeTypeEntity(
        string? name = null,
        HousingType housingType = HousingType.Undefined,
        params IHomeTypeSectionEntity[] segments)
    {
        ChangeDetails(name, housingType);
        _segments = segments.ToDictionary(x => x.SegmentType, x => x);
    }

    public HomeTypeId? Id { get; set; }

    public HomeTypeName? Name { get; private set; }

    public HousingType HousingType { get; private set; }

    public bool IsNew => Id.IsNotProvided();

    public void ChangeDetails(string? name, HousingType housingType)
    {
        Name = name != null ? new HomeTypeName(name) : null;
        HousingType = housingType;
    }

    public HomeTypeEntity Duplicate(HomeTypeName newName)
    {
        return new HomeTypeEntity(newName.Value, HousingType, _segments.Select(x => x.Value).ToArray());
    }

    public bool IsCompleted()
    {
        return Name.IsProvided() && _segments.All(x => x.Value.IsCompleted());
    }
}
