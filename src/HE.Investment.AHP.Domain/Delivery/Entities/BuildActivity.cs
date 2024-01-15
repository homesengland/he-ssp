using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class BuildActivity : ValueObject, IQuestion
{
    private readonly Tenure _tenure;

    private TypeOfHomes _typeOfHomes;

    public BuildActivity(Tenure tenure, TypeOfHomes typeOfHomes, BuildActivityType type)
    {
        _tenure = tenure;
        _typeOfHomes = typeOfHomes;
        Type = type;
    }

    public BuildActivity(Tenure tenure)
    {
        _tenure = tenure;
    }

    public BuildActivityType? Type { get; private set; }

    public static BuildActivityType[] GetAvailableTypeForRehab(Tenure tenure)
    {
        if (tenure.IsIn(Tenure.HomeOwnershipLongTermDisabilities))
        {
            return new[] { BuildActivityType.ExistingSatisfactory };
        }

        var availableTypeForRehab = new List<BuildActivityType>
        {
            BuildActivityType.AcquisitionAndWorksRehab,
            BuildActivityType.ExistingSatisfactory,
            BuildActivityType.PurchaseAndRepair,
            BuildActivityType.LeaseAndRepair,
            BuildActivityType.Reimprovement,
            BuildActivityType.Conversion,
            BuildActivityType.WorksOnlyRehab,
            BuildActivityType.RegenerationRehab,
        };

        if (tenure.IsIn(Tenure.SharedOwnership, Tenure.RentToBuy, Tenure.OlderPersonsSharedOwnership))
        {
            availableTypeForRehab.Remove(BuildActivityType.LeaseAndRepair);
        }

        return availableTypeForRehab.ToArray();
    }

    public static BuildActivityType[] GetAvailableTypeForNewBuild(Tenure tenure)
    {
        if (tenure.IsIn(Tenure.HomeOwnershipLongTermDisabilities))
        {
            return new[] { BuildActivityType.OffTheShelf };
        }

        return new[]
        {
            BuildActivityType.AcquisitionAndWorks,
            BuildActivityType.OffTheShelf,
            BuildActivityType.WorksOnly,
            BuildActivityType.LandInclusivePackage,
            BuildActivityType.Regeneration,
        };
    }

    public BuildActivityType[] GetAvailableTypes()
    {
        if (_typeOfHomes == TypeOfHomes.Rehab)
        {
            return GetAvailableTypeForRehab(_tenure);
        }

        return GetAvailableTypeForNewBuild(_tenure);
    }

    public void ClearAnswer(TypeOfHomes newTypeOfHome)
    {
        Type = null;
        _typeOfHomes = newTypeOfHome;
    }

    public bool IsAnswered()
    {
        return Type.IsProvided() && (
                (_typeOfHomes == TypeOfHomes.Rehab && GetAvailableTypeForRehab(_tenure).Contains(Type!.Value))
               || (_typeOfHomes == TypeOfHomes.NewBuild && GetAvailableTypeForNewBuild(_tenure).Contains(Type!.Value)));
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return _tenure;
        yield return _typeOfHomes;
    }
}
