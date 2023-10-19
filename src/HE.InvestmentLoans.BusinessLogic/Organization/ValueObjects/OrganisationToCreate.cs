using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

public class OrganisationToCreate : ValueObject
{
    public OrganisationToCreate(string name, OrganisationAddress address)
    {
        Build(name, address).CheckErrors();
    }

    public string Name { get; private set; }

    public OrganisationAddress Address { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
        yield return Address;
    }

    private OperationResult Build(string name, OrganisationAddress address)
    {
        var operationResult = OperationResult.New();

        Name = Validator
            .For(name, nameof(Name), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationName)
            .IsShortInput();

        Address = address;

        return operationResult;
    }
}
