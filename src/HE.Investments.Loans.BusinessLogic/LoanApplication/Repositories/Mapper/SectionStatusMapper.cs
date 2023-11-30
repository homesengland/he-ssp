using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories.Mapper;

public static class SectionStatusMapper
{
    public static SectionStatus Map(int? sectionStatus)
    {
        return sectionStatus switch
        {
            858110000 => SectionStatus.NotStarted,
            858110001 => SectionStatus.InProgress,
            858110002 => SectionStatus.Completed,
            858110003 => SectionStatus.Submitted,
            858110004 => SectionStatus.NotSubmitted,
            _ => SectionStatus.NotStarted,
        };
    }

    public static int Map(SectionStatus sectionStatus)
    {
        return sectionStatus switch
        {
            SectionStatus.NotStarted => 858110000,
            SectionStatus.InProgress => 858110001,
            SectionStatus.Completed => 858110002,
            SectionStatus.Submitted => 858110003,
            SectionStatus.NotSubmitted => 858110004,
            _ => 858110000,
        };
    }
}
