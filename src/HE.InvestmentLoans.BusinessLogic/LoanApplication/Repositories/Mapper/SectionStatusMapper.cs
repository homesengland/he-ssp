using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;

public static class SectionStatusMapper
{
    public static SectionStatus Map(int? sectionStatus)
    {
        return sectionStatus switch
        {
            858110000 => SectionStatus.NotStarted,
            858110001 => SectionStatus.InProgress,
            858110002 => SectionStatus.Completed,
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
            _ => 858110000,
        };
    }
}
