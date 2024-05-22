
extern alias Org;

using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;

extern alias Org;

public static class LocalAuthorities
{
    public static readonly LocalAuthority Oxford = new(new LocalAuthorityCode("7000178"), "Oxford");
}
