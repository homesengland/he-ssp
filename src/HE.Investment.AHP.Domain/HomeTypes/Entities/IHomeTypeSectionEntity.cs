namespace HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;

public interface IHomeTypeSectionEntity
{
    HomeTypeSectionType SectionType { get; }

    bool IsCompleted();
}
