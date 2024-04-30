using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.ModernMethodsConstruction)]
public class ModernMethodsConstructionSegmentEntity : DomainEntity, IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker;

    public ModernMethodsConstructionSegmentEntity(
        SiteUsingModernMethodsOfConstruction siteUsingModernMethodsOfConstruction,
        YesNoType modernMethodsConstructionApplied = YesNoType.Undefined,
        IEnumerable<ModernMethodsConstructionCategoriesType>? modernMethodsConstructionCategories = null,
        IEnumerable<ModernMethodsConstruction2DSubcategoriesType>? modernMethodsConstruction2DSubcategories = null,
        IEnumerable<ModernMethodsConstruction3DSubcategoriesType>? modernMethodsConstruction3DSubcategories = null)
    {
        SiteUsingModernMethodsOfConstruction = siteUsingModernMethodsOfConstruction;
        _modificationTracker = new ModificationTracker(() => SegmentModified?.Invoke());
        ModernMethodsConstructionApplied = modernMethodsConstructionApplied;
        ModernMethodsConstructionCategories = modernMethodsConstructionCategories?.ToList() ?? new List<ModernMethodsConstructionCategoriesType>();
        ModernMethodsConstruction2DSubcategories = modernMethodsConstruction2DSubcategories?.ToList() ?? new List<ModernMethodsConstruction2DSubcategoriesType>();
        ModernMethodsConstruction3DSubcategories = modernMethodsConstruction3DSubcategories?.ToList() ?? new List<ModernMethodsConstruction3DSubcategoriesType>();
    }

    public event EntityModifiedEventHandler SegmentModified;

    public bool IsModified => _modificationTracker.IsModified;

    public SiteUsingModernMethodsOfConstruction SiteUsingModernMethodsOfConstruction { get; }

    public YesNoType ModernMethodsConstructionApplied { get; private set; }

    public IReadOnlyCollection<ModernMethodsConstructionCategoriesType> ModernMethodsConstructionCategories { get; private set; }

    public IReadOnlyCollection<ModernMethodsConstruction2DSubcategoriesType> ModernMethodsConstruction2DSubcategories { get; private set; }

    public IReadOnlyCollection<ModernMethodsConstruction3DSubcategoriesType> ModernMethodsConstruction3DSubcategories { get; private set; }

    public void ChangeModernMethodsConstructionApplied(YesNoType modernMethodsConstructionApplied)
    {
        ModernMethodsConstructionApplied = _modificationTracker.Change(ModernMethodsConstructionApplied, modernMethodsConstructionApplied);

        if (ModernMethodsConstructionApplied == YesNoType.No)
        {
            ChangeModernMethodsConstructionCategories(Enumerable.Empty<ModernMethodsConstructionCategoriesType>());
        }
    }

    public void ChangeModernMethodsConstructionCategories(IEnumerable<ModernMethodsConstructionCategoriesType> modernMethodsConstructionCategories)
    {
        var uniqueModernMethodsConstructionCategories = modernMethodsConstructionCategories.Distinct().ToList();

        if (!ModernMethodsConstructionCategories.SequenceEqual(uniqueModernMethodsConstructionCategories))
        {
            ModernMethodsConstructionCategories = uniqueModernMethodsConstructionCategories;
            _modificationTracker.MarkAsModified();
        }

        if (!ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems))
        {
            ChangeModernMethodsConstruction3DSubcategories(Enumerable.IDE0301Empty<ModernMethodsConstruction3DSubcategoriesType>());
        }

        if (!ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems))
        {
            ChangeModernMethodsConstruction2DSubcategories(Enumerable.Empty<ModernMethodsConstruction2DSubcategoriesType>());
        }
    }

    public void ChangeModernMethodsConstruction2DSubcategories(IEnumerable<ModernMethodsConstruction2DSubcategoriesType> modernMethodsConstruction2DSubcategories)
    {
        var uniqueModernMethodsConstruction2DSubcategories = modernMethodsConstruction2DSubcategories.Distinct().ToList();

        if (!ModernMethodsConstruction2DSubcategories.SequenceEqual(uniqueModernMethodsConstruction2DSubcategories))
        {
            ModernMethodsConstruction2DSubcategories = uniqueModernMethodsConstruction2DSubcategories;
            _modificationTracker.MarkAsModified();
        }
    }

    public void ChangeModernMethodsConstruction3DSubcategories(IEnumerable<ModernMethodsConstruction3DSubcategoriesType> modernMethodsConstruction3DSubcategories)
    {
        var uniqueModernMethodsConstruction3DSubcategories = modernMethodsConstruction3DSubcategories.Distinct().ToList();

        if (!ModernMethodsConstruction3DSubcategories.SequenceEqual(uniqueModernMethodsConstruction3DSubcategories))
        {
            ModernMethodsConstruction3DSubcategories = uniqueModernMethodsConstruction3DSubcategories;
            _modificationTracker.MarkAsModified();
        }
    }

    public ModernMethodsConstructionSegmentEntity Duplicate()
    {
        return new ModernMethodsConstructionSegmentEntity(
            SiteUsingModernMethodsOfConstruction,
            ModernMethodsConstructionApplied,
            ModernMethodsConstructionCategories,
            ModernMethodsConstruction2DSubcategories,
            ModernMethodsConstruction3DSubcategories);
    }

    public bool IsRequired(HousingType housingType)
    {
        return SiteUsingModernMethodsOfConstruction == SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes;
    }

    public bool IsCompleted(HousingType housingType, Tenure tenure)
    {
        return ModernMethodsConstructionApplied != YesNoType.Undefined
               && BuildConditionalRouteCompletionPredicates().All(isCompleted => isCompleted());
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
    }

    private IEnumerable<Func<bool>> BuildConditionalRouteCompletionPredicates()
    {
        if (ModernMethodsConstructionApplied == YesNoType.Yes)
        {
            yield return () => ModernMethodsConstructionCategories.Any();
        }

        if (ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems))
        {
            yield return () => ModernMethodsConstruction3DSubcategories.Any();
        }

        if (ModernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems))
        {
            yield return () => ModernMethodsConstruction2DSubcategories.Any();
        }
    }
}
