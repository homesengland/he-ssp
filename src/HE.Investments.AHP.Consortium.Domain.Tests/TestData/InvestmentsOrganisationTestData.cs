extern alias Org;

using HE.Investments.Common.Contract;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestData;

public static class InvestmentsOrganisationTestData
{
    public static readonly InvestmentsOrganisation CactusDevelopments = new(
        new OrganisationId("00000000-0000-0000-0001-000000000000"),
        "Cactus Developments");

    public static readonly InvestmentsOrganisation MoralesEntertainment = new(
        new OrganisationId("00000000-0000-0000-0002-000000000000"),
        "Morales Entertainment");

    public static readonly InvestmentsOrganisation JjCompany = new(
        new OrganisationId("00000000-0000-0000-0003-000000000000"),
        "JJ wy:Company");
}
