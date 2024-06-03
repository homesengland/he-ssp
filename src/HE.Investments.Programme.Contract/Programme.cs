using HE.Investments.Programme.Contract.Enums;

namespace HE.Investments.Programme.Contract;

public record Programme(
    ProgrammeId Id,
    string ShortName,
    string Name,
    ProgrammeType ProgrammeType,
    DateRange ProgrammeDates,
    DateRange FundingDates,
    DateRange StartOnSiteDates,
    DateRange CompletionDates);
