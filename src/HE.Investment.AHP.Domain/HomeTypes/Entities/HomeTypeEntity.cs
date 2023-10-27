using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypeEntity
{
    private readonly IDictionary<HomeTypeSectionType, IHomeTypeSectionEntity> _sections;

    public HomeTypeEntity(
        string? name = null,
        params IHomeTypeSectionEntity[] sections)
    {
        ChangeName(name);
        _sections = sections.ToDictionary(x => x.SectionType, x => x);
    }

    public HomeTypeId? Id { get; set; }

    public HomeTypeName? Name { get; private set; }

    public bool IsNew => Id.IsNotProvided();

    public HousingTypeSectionEntity HousingTypeSectionEntity => (HousingTypeSectionEntity)_sections[HomeTypeSectionType.HousingType];

    public void ChangeName(string? name)
    {
        Name = name != null ? new HomeTypeName(name) : null;
    }

    public HomeTypeEntity Duplicate(HomeTypeName newName)
    {
        return new HomeTypeEntity(newName.Value, _sections.Select(x => x.Value).ToArray());
    }

    public bool IsCompleted()
    {
        return Name.IsProvided() && _sections.All(x => x.Value.IsCompleted());
    }
}
