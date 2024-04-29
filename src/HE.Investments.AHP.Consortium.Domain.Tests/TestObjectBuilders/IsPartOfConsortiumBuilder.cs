using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using Moq;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;

public class IsPartOfConsortiumBuilder
{
    private readonly Mock<IIsPartOfConsortium> _item;

    private IsPartOfConsortiumBuilder(Mock<IIsPartOfConsortium> item)
    {
        _item = item;
    }

    public static IsPartOfConsortiumBuilder New()
    {
        return new(new Mock<IIsPartOfConsortium>());
    }

    public IsPartOfConsortiumBuilder IsNotPartOfConsortium()
    {
        _item.Setup(x => x.IsPartOfConsortiumForProgramme(It.IsAny<ProgrammeId>(), It.IsAny<OrganisationId>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        return this;
    }

    public IsPartOfConsortiumBuilder IsPartOfConsortium()
    {
        _item.Setup(x => x.IsPartOfConsortiumForProgramme(It.IsAny<ProgrammeId>(), It.IsAny<OrganisationId>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        return this;
    }

    public IIsPartOfConsortium Build()
    {
        return _item.Object;
    }
}
