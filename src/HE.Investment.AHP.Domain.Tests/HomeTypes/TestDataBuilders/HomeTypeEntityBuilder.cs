using System.Reflection;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

public class HomeTypeEntityBuilder : TestObjectBuilder<HomeTypeEntityBuilder, HomeTypeEntity>
{
    public HomeTypeEntityBuilder()
        : base(new HomeTypeEntity(
            ApplicationBasicInfoTestData.AffordableRentInDraftState,
            "My home type",
            HousingType.General,
            SectionStatus.InProgress))
    {
    }

    protected override HomeTypeEntityBuilder Builder => this;

    public HomeTypeEntityBuilder WithName(string value) => SetProperty(x => x.Name, new HomeTypeName(value));

    public HomeTypeEntityBuilder WithStatus(SectionStatus value) => SetProperty(x => x.Status, value);

    public HomeTypeEntityBuilder WithSegments(params IHomeTypeSegmentEntity[] segments)
    {
        PrivatePropertySetter.SetPrivateField(
            Item,
            "_segments",
            segments.ToDictionary(x => x.GetType().GetCustomAttributes<HomeTypeSegmentTypeAttribute>().Single().SegmentType, x => x));
        return this;
    }
}
