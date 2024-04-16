using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

public class HomeInformationBuilder : TestObjectBuilder<HomeInformationBuilder, HomeInformationSegmentEntity>
{
    public HomeInformationBuilder()
        : base(new HomeInformationSegmentEntity(ApplicationBasicInfoTestData.AffordableRentInDraftState))
    {
    }

    protected override HomeInformationBuilder Builder => this;

    public HomeInformationBuilder WithNumberOfHomes(int value) => SetProperty(x => x.NumberOfHomes, new NumberOfHomes(value));
}
