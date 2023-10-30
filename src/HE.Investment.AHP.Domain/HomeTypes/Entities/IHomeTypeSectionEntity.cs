namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeSectionEntity
{
    HomeTypeSegmentType SegmentType { get; }

    bool IsCompleted();
}
