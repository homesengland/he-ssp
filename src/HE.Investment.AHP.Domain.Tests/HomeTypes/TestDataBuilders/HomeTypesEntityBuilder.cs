using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.Site.TestData;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

public class HomeTypesEntityBuilder : TestObjectBuilder<HomeTypesEntityBuilder, HomeTypesEntity>
{
    public HomeTypesEntityBuilder()
        : base(new HomeTypesEntity(
            ApplicationBasicInfoTestData.AffordableRentInDraftState,
            SiteBasicInfoTestData.DefaultSite(ApplicationBasicInfoTestData.AffordableRentInDraftState.SiteId),
            Enumerable.Empty<HomeTypeEntity>(),
            SectionStatus.NotStarted))
    {
    }

    protected override HomeTypesEntityBuilder Builder => this;

    public HomeTypesEntityBuilder WithHomeTypes(params HomeTypeEntity[] homeTypes)
    {
        PrivatePropertySetter.SetPrivateField(Item, "_homeTypes", Item.HomeTypes.Cast<HomeTypeEntity>().Concat(homeTypes).ToList());
        return this;
    }

    public HomeTypesEntityBuilder WithStatus(SectionStatus value) => SetProperty(x => x.Status, value);
}
