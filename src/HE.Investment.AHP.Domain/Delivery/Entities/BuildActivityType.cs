using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class BuildActivityType : ValueObject, IQuestion
{
    private readonly TypeOfHomes _typeOfHomes;

    public BuildActivityType(TypeOfHomes typeOfHomes, BuildActivityTypeForNewBuild newBuild)
    {
        if (typeOfHomes is not TypeOfHomes.NewBuild)
        {
            throw new DomainValidationException("Type of homes must be NewBuild or Rehab when BuildActivityTypeForNewBuild is provided.");
        }

        _typeOfHomes = typeOfHomes;
        NewBuild = newBuild;
    }

    public BuildActivityType(TypeOfHomes typeOfHomes, BuildActivityTypeForRehab rehab)
    {
        if (typeOfHomes is not TypeOfHomes.Rehab)
        {
            throw new DomainValidationException("Type of homes must be Rehab when BuildActivityTypeForRehab is provided.");
        }

        _typeOfHomes = typeOfHomes;
        Rehab = rehab;
    }

    public BuildActivityType()
    {
    }

    public BuildActivityTypeForNewBuild? NewBuild { get; private set; }

    public BuildActivityTypeForRehab? Rehab { get; private set; }

    public void ClearAnswer()
    {
        NewBuild = null;
        Rehab = null;
    }

    public bool IsAnswered()
    {
        return (_typeOfHomes == TypeOfHomes.Rehab && Rehab.IsProvided())
               || (_typeOfHomes == TypeOfHomes.NewBuild && NewBuild.IsProvided());
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return NewBuild;
        yield return Rehab;
    }
}
