namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeSegmentEntity
{
    HomeTypeSegmentType SegmentType { get; }

    bool IsCompleted();
}
