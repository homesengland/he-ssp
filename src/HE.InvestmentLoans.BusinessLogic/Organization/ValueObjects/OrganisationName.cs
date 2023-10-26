using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

public class OrganisationName : ValueObject
{
    public OrganisationName(string name)
    {
        Build(name).CheckErrors();
    }

    public string Name { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
    }

    private OperationResult Build(string name)
    {
        var operationResult = OperationResult.New();

        Name = Validator
            .For(name, nameof(Name), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationName)
            .IsShortInput();

        return operationResult;
    }
}
