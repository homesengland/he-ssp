using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using GeographicFocus = HE.Investments.FrontDoor.Shared.Project.Contract.ProjectGeographicFocus;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class ProjectGeographicFocus : ValueObject, IQuestion
{
    public ProjectGeographicFocus(GeographicFocus geographicFocus)
    {
        if (geographicFocus == GeographicFocus.Undefined)
        {
            OperationResult.ThrowValidationError(nameof(GeographicFocus), ValidationErrorMessage.MustBeSelected("geographic focus of the project"));
        }

        GeographicFocus = geographicFocus;
    }

    private ProjectGeographicFocus()
    {
        GeographicFocus = GeographicFocus.Undefined;
    }

    public GeographicFocus GeographicFocus { get; }

    public static ProjectGeographicFocus Empty() => new();

    public static ProjectGeographicFocus? Create(GeographicFocus? geographicFocus) =>
        geographicFocus.IsProvided() ? new ProjectGeographicFocus(geographicFocus!.Value) : null;

    public bool IsAnswered()
    {
        return GeographicFocus != GeographicFocus.Undefined;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return GeographicFocus;
    }
}
