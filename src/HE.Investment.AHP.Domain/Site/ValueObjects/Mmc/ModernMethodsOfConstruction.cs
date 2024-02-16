using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstruction : ValueObject, IQuestion
{
    private readonly List<ModernMethodsConstructionCategoriesType> _modernMethodsConstructionCategories;
    private readonly List<ModernMethodsConstruction2DSubcategoriesType> _modernMethodsConstruction2DSubcategories;
    private readonly List<ModernMethodsConstruction3DSubcategoriesType> _modernMethodsConstruction3DSubcategories;

    public ModernMethodsOfConstruction(
        IEnumerable<ModernMethodsConstructionCategoriesType>? modernMethodsConstructionCategories = null,
        IEnumerable<ModernMethodsConstruction2DSubcategoriesType>? modernMethodsConstruction2DSubcategories = null,
        IEnumerable<ModernMethodsConstruction3DSubcategoriesType>? modernMethodsConstruction3DSubcategories = null)
    {
        _modernMethodsConstructionCategories = modernMethodsConstructionCategories?.ToList() ?? new List<ModernMethodsConstructionCategoriesType>();
        _modernMethodsConstruction2DSubcategories = modernMethodsConstruction2DSubcategories?.ToList() ?? new List<ModernMethodsConstruction2DSubcategoriesType>();
        _modernMethodsConstruction3DSubcategories = modernMethodsConstruction3DSubcategories?.ToList() ?? new List<ModernMethodsConstruction3DSubcategoriesType>();

        if (_modernMethodsConstruction2DSubcategories.Any() &&
            !_modernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems))
        {
            throw new DomainValidationException("2D subcategory can be selected only when 2D category is selected.");
        }

        if (_modernMethodsConstruction3DSubcategories.Any() &&
            !_modernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems))
        {
            throw new DomainValidationException("3D subcategory can be selected only when 3D category is selected.");
        }
    }

    public IReadOnlyCollection<ModernMethodsConstructionCategoriesType> ModernMethodsConstructionCategories => _modernMethodsConstructionCategories;

    public IReadOnlyCollection<ModernMethodsConstruction2DSubcategoriesType> ModernMethodsConstruction2DSubcategories => _modernMethodsConstruction2DSubcategories;

    public IReadOnlyCollection<ModernMethodsConstruction3DSubcategoriesType> ModernMethodsConstruction3DSubcategories => _modernMethodsConstruction3DSubcategories;

    public bool IsAnswered()
    {
        if (_modernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems))
        {
            return ModernMethodsConstruction3DSubcategories.Any();
        }

        if (_modernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems))
        {
            return ModernMethodsConstruction2DSubcategories.Any();
        }

        return ModernMethodsConstructionCategories.Any();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ModernMethodsConstructionCategories;
        yield return ModernMethodsConstruction2DSubcategories;
        yield return ModernMethodsConstruction3DSubcategories;
    }
}
