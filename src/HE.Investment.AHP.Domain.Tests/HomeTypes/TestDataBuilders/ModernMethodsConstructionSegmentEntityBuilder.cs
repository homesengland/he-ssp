using System.Collections.ObjectModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

public class ModernMethodsConstructionSegmentEntityBuilder
    : TestObjectBuilder<ModernMethodsConstructionSegmentEntityBuilder, ModernMethodsConstructionSegmentEntity>
{
    public ModernMethodsConstructionSegmentEntityBuilder()
        : base(new ModernMethodsConstructionSegmentEntity(SiteUsingModernMethodsOfConstruction.No))
    {
    }

    protected override ModernMethodsConstructionSegmentEntityBuilder Builder => this;

    public ModernMethodsConstructionSegmentEntityBuilder WithMmcIsApplied(YesNoType value) => SetProperty(x => x.ModernMethodsConstructionApplied, value);

    public ModernMethodsConstructionSegmentEntityBuilder WithMmcCategories(params ModernMethodsConstructionCategoriesType[] value) => SetProperty(x => x.ModernMethodsConstructionCategories, value);

    public ModernMethodsConstructionSegmentEntityBuilder WithMmc3DCategories(params ModernMethodsConstruction3DSubcategoriesType[] value) => SetProperty(x => x.ModernMethodsConstruction3DSubcategories, value);

    public ModernMethodsConstructionSegmentEntityBuilder WithMmc2DCategories(params ModernMethodsConstruction2DSubcategoriesType[] value) => SetProperty(x => x.ModernMethodsConstruction2DSubcategories, value);
}
