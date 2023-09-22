using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class AdditionalProjects : ValueObject
{
    public AdditionalProjects(bool isThereAnyAdditionalProject)
    {
        IsThereAnyAdditionalProject = isThereAnyAdditionalProject;
    }

    public bool IsThereAnyAdditionalProject { get; }

    public static AdditionalProjects New(bool isThereAnyAdditionalProject) => new(isThereAnyAdditionalProject);

    public static AdditionalProjects FromString(string isThereAnyAdditionalProject)
    {
        return new AdditionalProjects(isThereAnyAdditionalProject == CommonResponse.Yes);
    }

    public override string ToString()
    {
        return IsThereAnyAdditionalProject.ToString().ToLowerInvariant();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsThereAnyAdditionalProject;
    }
}
