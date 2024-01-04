using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.ModernMethodsConstruction)]
public class ModernMethodsConstructionEntity : IHomeTypeSegmentEntity
{
    private readonly List<ModernMethodsConstructionCategoriesType> _modernMethodsConstructionCategories;
    private readonly List<ModernMethodsConstruction2DSubcategoriesType> _modernMethodsConstruction2DSubcategories;
    private readonly List<ModernMethodsConstruction3DSubcategoriesType> _modernMethodsConstruction3DSubcategories;

    private readonly ModificationTracker _modificationTracker;

    public ModernMethodsConstructionEntity(
        YesNoType modernMethodsConstructionApplied = YesNoType.Undefined,
        IEnumerable<ModernMethodsConstructionCategoriesType>? modernMethodsConstructionCategories = null,
        IEnumerable<ModernMethodsConstruction2DSubcategoriesType>? modernMethodsConstruction2DSubcategories = null,
        IEnumerable<ModernMethodsConstruction3DSubcategoriesType>? modernMethodsConstruction3DSubcategories = null)
    {
        _modificationTracker = new ModificationTracker(() => SegmentModified?.Invoke());
        ModernMethodsConstructionApplied = modernMethodsConstructionApplied;
        _modernMethodsConstructionCategories = modernMethodsConstructionCategories?.ToList() ?? new List<ModernMethodsConstructionCategoriesType>();
        _modernMethodsConstruction2DSubcategories = modernMethodsConstruction2DSubcategories?.ToList() ?? new List<ModernMethodsConstruction2DSubcategoriesType>();
        _modernMethodsConstruction3DSubcategories = modernMethodsConstruction3DSubcategories?.ToList() ?? new List<ModernMethodsConstruction3DSubcategoriesType>();
    }

    public event EntityModifiedEventHandler SegmentModified;

    public bool IsModified => _modificationTracker.IsModified;

    public YesNoType ModernMethodsConstructionApplied { get; private set; }

    public IReadOnlyCollection<ModernMethodsConstructionCategoriesType> ModernMethodsConstructionCategories => _modernMethodsConstructionCategories;

    public IReadOnlyCollection<ModernMethodsConstruction2DSubcategoriesType> ModernMethodsConstruction2DSubcategories => _modernMethodsConstruction2DSubcategories;

    public IReadOnlyCollection<ModernMethodsConstruction3DSubcategoriesType> ModernMethodsConstruction3DSubcategories => _modernMethodsConstruction3DSubcategories;

    public void ChangeModernMethodsConstructionApplied(YesNoType modernMethodsConstructionApplied)
    {
        ModernMethodsConstructionApplied = _modificationTracker.Change(ModernMethodsConstructionApplied, modernMethodsConstructionApplied);
    }

    public void ChangeModernMethodsConstructionCategories(IEnumerable<ModernMethodsConstructionCategoriesType> modernMethodsConstructionCategories)
    {
        var uniqueModernMethodsConstructionCategories = modernMethodsConstructionCategories.Distinct().ToList();

        if (!_modernMethodsConstructionCategories.SequenceEqual(uniqueModernMethodsConstructionCategories))
        {
            _modernMethodsConstructionCategories.Clear();
            _modernMethodsConstructionCategories.AddRange(uniqueModernMethodsConstructionCategories);
            _modificationTracker.MarkAsModified();
        }
    }

    public void ChangeModernMethodsConstruction2DSubcategories(IEnumerable<ModernMethodsConstruction2DSubcategoriesType> modernMethodsConstruction2DSubcategories)
    {
        var uniqueModernMethodsConstruction2DSubcategories = modernMethodsConstruction2DSubcategories.Distinct().ToList();

        if (!_modernMethodsConstruction2DSubcategories.SequenceEqual(uniqueModernMethodsConstruction2DSubcategories))
        {
            _modernMethodsConstruction2DSubcategories.Clear();
            _modernMethodsConstruction2DSubcategories.AddRange(uniqueModernMethodsConstruction2DSubcategories);
            _modificationTracker.MarkAsModified();
        }
    }

    public void ChangeModernMethodsConstruction3DSubcategories(IEnumerable<ModernMethodsConstruction3DSubcategoriesType> modernMethodsConstruction3DSubcategories)
    {
        var uniqueModernMethodsConstruction3DSubcategories = modernMethodsConstruction3DSubcategories.Distinct().ToList();

        if (!_modernMethodsConstruction3DSubcategories.SequenceEqual(uniqueModernMethodsConstruction3DSubcategories))
        {
            _modernMethodsConstruction3DSubcategories.Clear();
            _modernMethodsConstruction3DSubcategories.AddRange(uniqueModernMethodsConstruction3DSubcategories);
            _modificationTracker.MarkAsModified();
        }
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new ModernMethodsConstructionEntity(
            ModernMethodsConstructionApplied,
            ModernMethodsConstructionCategories,
            ModernMethodsConstruction2DSubcategories,
            ModernMethodsConstruction3DSubcategories);
    }

    public bool IsRequired(HousingType housingType)
    {
        return true;
    }

    public bool IsCompleted(HousingType housingType, Tenure tenure)
    {
        return true;

        // TODO crm missing - returning true not to block section completion
        // return ModernMethodsConstructionApplied != YesNoType.Undefined
        //       && BuildConditionalRouteCompletionPredicates().All(isCompleted => isCompleted());
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
    }
}
