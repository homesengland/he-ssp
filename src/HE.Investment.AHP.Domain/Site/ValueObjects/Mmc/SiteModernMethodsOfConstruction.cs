using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class SiteModernMethodsOfConstruction : ValueObject, IQuestion
{
    public SiteModernMethodsOfConstruction(
        SiteUsingModernMethodsOfConstruction? siteUsingModernMethodsOfConstruction = null,
        ModernMethodsOfConstructionInformation? information = null,
        ModernMethodsOfConstructionFutureAdoption? futureAdoption = null,
        ModernMethodsOfConstruction? modernMethodsOfConstruction = null)
    {
        SiteUsingModernMethodsOfConstruction = siteUsingModernMethodsOfConstruction;
        Information = information;
        FutureAdoption = futureAdoption;
        ModernMethodsOfConstruction = modernMethodsOfConstruction;
    }

    public SiteUsingModernMethodsOfConstruction? SiteUsingModernMethodsOfConstruction { get; }

    public ModernMethodsOfConstructionInformation? Information { get; }

    public ModernMethodsOfConstructionFutureAdoption? FutureAdoption { get; }

    public ModernMethodsOfConstruction? ModernMethodsOfConstruction { get; }

    public static SiteModernMethodsOfConstruction Create(SiteModernMethodsOfConstruction old, SiteUsingModernMethodsOfConstruction? siteUsingModernMethodsOfConstruction)
    {
        if (siteUsingModernMethodsOfConstruction is not null and not Contract.Site.SiteUsingModernMethodsOfConstruction.No)
        {
            return new SiteModernMethodsOfConstruction(siteUsingModernMethodsOfConstruction, old.Information, null, old.ModernMethodsOfConstruction);
        }

        return new SiteModernMethodsOfConstruction(siteUsingModernMethodsOfConstruction, null, old.FutureAdoption, null);
    }

    public static SiteModernMethodsOfConstruction Create(SiteModernMethodsOfConstruction old, IList<ModernMethodsConstructionCategoriesType>? modernMethodsConstructionCategories)
    {
        if (modernMethodsConstructionCategories == null)
        {
            return new SiteModernMethodsOfConstruction(old.SiteUsingModernMethodsOfConstruction, old.Information, old.FutureAdoption);
        }

        var modernMethodsConstruction = new ModernMethodsOfConstruction(
            modernMethodsConstructionCategories,
            modernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems) ? old.ModernMethodsOfConstruction?.ModernMethodsConstruction2DSubcategories : null,
            modernMethodsConstructionCategories.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems) ? old.ModernMethodsOfConstruction?.ModernMethodsConstruction3DSubcategories : null);

        return new SiteModernMethodsOfConstruction(old.SiteUsingModernMethodsOfConstruction, old.Information, old.FutureAdoption, modernMethodsConstruction);
    }

    public static SiteModernMethodsOfConstruction Create(SiteModernMethodsOfConstruction old, IList<ModernMethodsConstruction2DSubcategoriesType>? modernMethodsConstruction2DSubcategories)
    {
        var modernMethodsConstruction = new ModernMethodsOfConstruction(
            old.ModernMethodsOfConstruction?.ModernMethodsConstructionCategories,
            modernMethodsConstruction2DSubcategories,
            old.ModernMethodsOfConstruction?.ModernMethodsConstruction3DSubcategories);

        return new SiteModernMethodsOfConstruction(old.SiteUsingModernMethodsOfConstruction, old.Information, old.FutureAdoption, modernMethodsConstruction);
    }

    public static SiteModernMethodsOfConstruction Create(SiteModernMethodsOfConstruction old, IList<ModernMethodsConstruction3DSubcategoriesType>? modernMethodsConstruction3DSubcategories)
    {
        var modernMethodsConstruction = new ModernMethodsOfConstruction(
            old.ModernMethodsOfConstruction?.ModernMethodsConstructionCategories,
            old.ModernMethodsOfConstruction?.ModernMethodsConstruction2DSubcategories,
            modernMethodsConstruction3DSubcategories);

        return new SiteModernMethodsOfConstruction(old.SiteUsingModernMethodsOfConstruction, old.Information, old.FutureAdoption, modernMethodsConstruction);
    }

    public static SiteModernMethodsOfConstruction Create(SiteModernMethodsOfConstruction old, ModernMethodsOfConstructionInformation? information)
    {
        return new SiteModernMethodsOfConstruction(old.SiteUsingModernMethodsOfConstruction, information, old.FutureAdoption, old.ModernMethodsOfConstruction);
    }

    public static SiteModernMethodsOfConstruction Create(SiteModernMethodsOfConstruction old, ModernMethodsOfConstructionFutureAdoption? futureAdoption)
    {
        return new SiteModernMethodsOfConstruction(old.SiteUsingModernMethodsOfConstruction, old.Information, futureAdoption, old.ModernMethodsOfConstruction);
    }

    public bool IsAnswered()
    {
        if (SiteUsingModernMethodsOfConstruction == Contract.Site.SiteUsingModernMethodsOfConstruction.No)
        {
            return FutureAdoption != null && FutureAdoption.IsAnswered();
        }

        if (SiteUsingModernMethodsOfConstruction is Contract.Site.SiteUsingModernMethodsOfConstruction.Yes or Contract.Site.SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
        {
            return Information != null && Information.IsAnswered() && ModernMethodsOfConstruction != null && ModernMethodsOfConstruction.IsAnswered();
        }

        return false;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return SiteUsingModernMethodsOfConstruction;
        yield return Information;
        yield return FutureAdoption;
        yield return ModernMethodsOfConstruction;
    }
}
