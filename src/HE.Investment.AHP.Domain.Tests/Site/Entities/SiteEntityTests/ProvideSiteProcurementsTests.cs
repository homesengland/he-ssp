using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class ProvideSiteProcurementsTests
{
    [Fact]
    public void ShouldSaveProcurements_WhenProcurementsAreProvided()
    {
        // given
        var procurements = new SiteProcurements([SiteProcurement.BulkPurchaseOfComponents, SiteProcurement.Other]);
        var siteEntity = SiteEntityBuilder.New().WithProcurements(procurements).Build();

        // when
        siteEntity.ProvideProcurement(procurements);

        // when
        siteEntity.Procurements.Should().Be(procurements);
    }

    [Fact]
    public void ShouldThrowException_WhenNoneAndAnyOtherOptionWasSelected()
    {
        // possible with disabled javascript
        // given
        var procurements = new List<SiteProcurement> { SiteProcurement.None, SiteProcurement.BulkPurchaseOfComponents };

        // when
        var act = () => new SiteProcurements(procurements);

        // then
        act.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.InvalidValue);
    }
}
