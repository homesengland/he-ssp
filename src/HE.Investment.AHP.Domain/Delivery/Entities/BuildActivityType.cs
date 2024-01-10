using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class BuildActivityType : ValueObject, IQuestion
{
    public BuildActivityType(TypeOfHomes typeOfHomes, BuildActivityTypeForNewBuild newBuild)
    {
        if (typeOfHomes is not TypeOfHomes.NewBuild)
        {
            throw new DomainValidationException("Type of homes must be NewBuild or Rehab when BuildActivityTypeForNewBuild is provided.");
        }

        TypeOfHomes = typeOfHomes;
        NewBuild = newBuild;
    }

    public BuildActivityType(TypeOfHomes typeOfHomes, BuildActivityTypeForRehab rehab)
    {
        if (typeOfHomes is not TypeOfHomes.Rehab)
        {
            throw new DomainValidationException("Type of homes must be Rehab when BuildActivityTypeForRehab is provided.");
        }

        TypeOfHomes = typeOfHomes;
        Rehab = rehab;
    }

    public BuildActivityType()
    {
    }

    public BuildActivityTypeForNewBuild? NewBuild { get; private set; }

    public BuildActivityTypeForRehab? Rehab { get; private set; }

    public TypeOfHomes TypeOfHomes { get; }

    public bool IsAnswered()
    {
        return (TypeOfHomes == TypeOfHomes.Rehab && Rehab.IsProvided())
               || (TypeOfHomes == TypeOfHomes.NewBuild && NewBuild.IsProvided());
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return NewBuild;
        yield return Rehab;
    }
}
