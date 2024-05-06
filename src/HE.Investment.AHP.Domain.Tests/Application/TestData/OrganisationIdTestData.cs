using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Tests.Application.TestData;

public static class OrganisationIdTestData
{
    public static readonly OrganisationId OrganisationIdOne = new(Guid.NewGuid().ToString());
}
