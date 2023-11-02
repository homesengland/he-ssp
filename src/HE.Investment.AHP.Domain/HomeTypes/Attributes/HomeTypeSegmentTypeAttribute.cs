using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class HomeTypeSegmentTypeAttribute : Attribute
{
    public HomeTypeSegmentTypeAttribute(HomeTypeSegmentType segmentType)
    {
        SegmentType = segmentType;
    }

    public HomeTypeSegmentType SegmentType { get; }
}
