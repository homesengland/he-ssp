using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Organisation.Tests.TestAssertions;

public static class OrganisationSearchItemAssertions
{
    public static GenericCollectionAssertions<OrganisationSearchItem> ContainSingle(
        this GenericCollectionAssertions<OrganisationSearchItem> toAssert,
        string organisationName,
        string? companyRegistrationNumber,
        string? organisationId,
        bool existInCrm)
    {
        toAssert.ContainSingle(x =>
            x.Name == organisationName &&
            x.CompanyNumber == companyRegistrationNumber &&
            x.OrganisationId == organisationId &&
            x.ExistsInCrm == existInCrm);
        return toAssert;
    }

    public static void ShouldBe(
        this OrganisationSearchItem toAssert,
        string organisationName,
        string? companyRegistrationNumber,
        string? organisationId,
        bool existInCrm)
    {
        toAssert.CompanyNumber.Should().Be(companyRegistrationNumber);
        toAssert.Name.Should().Be(organisationName);
        toAssert.ExistsInCrm.Should().Be(existInCrm);
        toAssert.OrganisationId.Should().Be(organisationId);
    }
}
