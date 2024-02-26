using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using HE.Investments.Common.Extensions;

namespace HE.Investments.IntegrationTestsFramework.Assertions;

public static class SummaryFluentAssertions
{
    public static AndConstraint<StringAssertions> WithValue<TCollection, TAssertions, TEnum>(
        this WhoseValueConstraint<TCollection, string, string, TAssertions> summary,
        TEnum expectedValue)
        where TCollection : IEnumerable<KeyValuePair<string, string>>
        where TAssertions : GenericDictionaryAssertions<TCollection, string, string, TAssertions>
        where TEnum : struct, Enum
    {
        return summary.WithValue(expectedValue.GetDescription());
    }

    public static AndConstraint<StringAssertions> WithValue<TCollection, TAssertions>(
        this WhoseValueConstraint<TCollection, string, string, TAssertions> summary,
        string expectedValue)
        where TCollection : IEnumerable<KeyValuePair<string, string>>
        where TAssertions : GenericDictionaryAssertions<TCollection, string, string, TAssertions>
    {
        return summary.WhoseValue.Should().Be(expectedValue);
    }

    public static AndConstraint<StringAssertions> WithOnlyValues<TCollection, TAssertions, TEnum>(
        this WhoseValueConstraint<TCollection, string, string, TAssertions> summary,
        IEnumerable<TEnum> values)
        where TCollection : IEnumerable<KeyValuePair<string, string>>
        where TAssertions : GenericDictionaryAssertions<TCollection, string, string, TAssertions>
        where TEnum : struct, Enum
    {
        var result = summary.WhoseValue.Should().NotBeNullOrWhiteSpace();
        var options = result.And.Subject.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x);
        var expectedOptions = values.Select(x => x.GetDescription())
            .OrderBy(x => x);

        options.Should().BeEquivalentTo(expectedOptions);

        return result;
    }
}
