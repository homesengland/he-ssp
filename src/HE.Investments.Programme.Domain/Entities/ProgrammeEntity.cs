using System.Globalization;
using System.Text;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Domain.ValueObjects;

namespace HE.Investments.Programme.Domain.Entities;

public class ProgrammeEntity
{
    public ProgrammeEntity(
        ProgrammeId id,
        string name,
        ProgrammeDates programmeDates,
        ProgrammeDates fundingDates,
        ProgrammeDates startOnSiteDates,
        ProgrammeDates completionDates)
    {
        Id = id;
        ShortName = ParseShortName(name);
        Name = name;
        ProgrammeDates = programmeDates;
        FundingDates = fundingDates;
        StartOnSiteDates = startOnSiteDates;
        CompletionDates = completionDates;
    }

    public ProgrammeId Id { get; }

    public string ShortName { get; }

    public string Name { get; }

    public ProgrammeType ProgrammeType => ProgrammeType.Ahp;

    public ProgrammeDates ProgrammeDates { get; }

    public ProgrammeDates FundingDates { get; }

    public ProgrammeDates StartOnSiteDates { get; }

    public ProgrammeDates CompletionDates { get; }

    public bool IsOpenForApplications(DateOnly today) => ProgrammeDates.IsWithinRange(today);

    private static string ParseShortName(string fullName)
    {
        var shortNameBuilder = new StringBuilder();
        var words = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            if (word.Contains("20"))
            {
                shortNameBuilder.Append(CultureInfo.InvariantCulture, $" {word.Replace("20", string.Empty)} ");
            }
            else
            {
                shortNameBuilder.Append(word[0]);
            }
        }

        return shortNameBuilder.ToString();
    }
}
