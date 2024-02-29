using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Helpers;

namespace HE.Investments.IntegrationTestsFramework.Assertions;

public static class SummaryFluentAssertions
{
    public static AndConstraint<StringAssertions> WithValue<TCollection, TAssertions, TEnum>(
        this WhoseValueConstraint<TCollection, string, SummaryItem, TAssertions> summary,
        TEnum expectedValue)
        where TCollection : IEnumerable<KeyValuePair<string, SummaryItem>>
        where TAssertions : GenericDictionaryAssertions<TCollection, string, SummaryItem, TAssertions>
        where TEnum : struct, Enum
    {
        return summary.WithValue(expectedValue.GetDescription());
    }

    public static AndConstraint<StringAssertions> WithValue<TCollection, TAssertions>(
        this WhoseValueConstraint<TCollection, string, SummaryItem, TAssertions> summary,
        string expectedValue)
        where TCollection : IEnumerable<KeyValuePair<string, SummaryItem>>
        where TAssertions : GenericDictionaryAssertions<TCollection, string, SummaryItem, TAssertions>
    {
        return summary.WhoseValue.Value.Should().Be(expectedValue);
    }

    public static AndConstraint<StringAssertions> WithOnlyValues<TCollection, TAssertions, TEnum>(
        this WhoseValueConstraint<TCollection, string, SummaryItem, TAssertions> summary,
        IEnumerable<TEnum> values)
        where TCollection : IEnumerable<KeyValuePair<string, SummaryItem>>
        where TAssertions : GenericDictionaryAssertions<TCollection, string, SummaryItem, TAssertions>
        where TEnum : struct, Enum
    {
        var result = summary.WhoseValue.Value.Should().NotBeNullOrWhiteSpace();
        var options = result.And.Subject.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x);
        var expectedOptions = values.Select(x => x.GetDescription())
            .OrderBy(x => x);

        options.Should().BeEquivalentTo(expectedOptions);

        return result;
    }
}
