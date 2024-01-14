using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Common.Tests.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
internal static class ProjectDateTestData
{
    public static readonly (string Year, string Month, string Day) IncorrectDateAsStrings = DateTimeTestData.IncorrectDateAsStrings;

    public static readonly (string Year, string Month, string Day) CorrectDateAsStrings = DateTimeTestData.CorrectDateAsStrings;

    public static readonly DateTime CorrectDateTime = new(2022, 1, 31, 0, 0, 0, DateTimeKind.Unspecified);

    public static readonly ProjectDate CorrectDate = new(new DateTime(2022, 1, 31, 0, 0, 0, DateTimeKind.Unspecified));

    public static readonly ProjectDate OtherCorrectDate = new(new DateTime(2022, 3, 31, 0, 0, 0, DateTimeKind.Unspecified));
}
