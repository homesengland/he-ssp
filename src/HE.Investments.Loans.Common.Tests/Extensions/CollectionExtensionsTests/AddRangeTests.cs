using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.Investments.Loans.Common.Tests.Extensions.CollectionExtensionsTests;

[SuppressMessage("Performance", "CA1861", Justification = "Reviewed")]
public class AddRangeTests
{
    [TestMethod]
    public void ShouldNotThrowException_WhenNullIsPassedAsParameter()
    {
        // given
        IList<int> listWithOneElement =
            [
                1,
            ];

        // when
        listWithOneElement.AddRange(null!);

        // then
        listWithOneElement.Should().OnlyContain(x => x == 1);
    }

    [TestMethod]
    public void ShouldAddTwoElementsToIList_WhenAddingElementsArePassedAsList()
    {
        // given
        IList<int> listWithTwoElements =
            [
                1,
                2,
            ];

        var elementsToAdd = new List<int>
            {
                5,
                4,
                4,
            };

        // when
        listWithTwoElements.AddRange(elementsToAdd);

        // then
        listWithTwoElements.Should().OnlyContain(
            x => new[]
            {
                    1,
                    2,
                    4,
                    5,
            }.Contains(x)).And.HaveCount(5);
    }

    [TestMethod]
    public void ShouldAddElementsToIList_WhenAddingElementsArePassedAsArray()
    {
        // given
        IList<int> listWithTwoElements =
            [
                1,
                2,
            ];

        var elementsToAdd = new[]
        {
                5,
                4,
                4,
        };

        // when
        listWithTwoElements.AddRange(elementsToAdd);

        // then
        listWithTwoElements.Should().OnlyContain(
            x => new[]
            {
                    1,
                    2,
                    4,
                    5,
            }.Contains(x)).And.HaveCount(5);
    }

    [TestMethod]
    public void ShouldAddElementsToCollection()
    {
        // given
        IList<int> listWithTwoElements = new Collection<int>(
            [
                1,
                2,
            ]);

        var elementsToAdd = new List<int>
            {
                5,
                4,
                4,
            };

        // when
        listWithTwoElements.AddRange(elementsToAdd);

        // then
        listWithTwoElements.Should().OnlyContain(
            x => new[]
            {
                    1,
                    2,
                    4,
                    5,
            }.Contains(x)).And.HaveCount(5);
    }

    [TestMethod]
    public void ShouldThrowException_WhenAddingToNullObject()
    {
        // when
        Action action = () => ((IList<int>)null!).AddRange(null!);

        // then
        action.Should().Throw<ArgumentNullException>();
    }
}
