using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.WWW.Workflows;

public class ProjectWorkflow : EncodedStateRouting<ProjectWorkflowState>
{
    private readonly ProjectDetails _model;

    public ProjectWorkflow(ProjectWorkflowState currentWorkflowState, ProjectDetails model, bool isLocked = false)
        : base(currentWorkflowState, isLocked)
    {
        _model = model;
        ConfigureTransitions();
    }

    public override bool CanBeAccessed(ProjectWorkflowState state, bool? isReadOnlyMode = null)
    {
        var isStateEligible = BuildDeadEndConditions(state).All(isValid => isValid());
        return isStateEligible && state switch
        {
            ProjectWorkflowState.EnglandHousingDelivery => true,
            ProjectWorkflowState.NotEligibleForAnything => !IsEligibleForAnything(),
            ProjectWorkflowState.Name => true,
            ProjectWorkflowState.SupportRequiredActivities => true,
            ProjectWorkflowState.Infrastructure => IsActivityOnly(SupportActivityType.ProvidingInfrastructure),
            ProjectWorkflowState.Tenure => IsActivityOnly(SupportActivityType.DevelopingHomes),
            ProjectWorkflowState.OrganisationHomesBuilt => IsActivityOnly(SupportActivityType.DevelopingHomes),
            ProjectWorkflowState.IdentifiedSite => true,
            ProjectWorkflowState.StartSiteDetails => IsSiteIdentified(),
            ProjectWorkflowState.LastSiteDetails => IsSiteIdentified(),
            ProjectWorkflowState.GeographicFocus => IsSiteNotIdentified(),
            ProjectWorkflowState.Region => IsSiteNotIdentified() && _model.GeographicFocus is ProjectGeographicFocus.Regional,
            ProjectWorkflowState.LocalAuthoritySearch => IsSiteNotIdentified() && _model.GeographicFocus is ProjectGeographicFocus.SpecificLocalAuthority,
            ProjectWorkflowState.LocalAuthorityResult => IsSiteNotIdentified() && _model.GeographicFocus is ProjectGeographicFocus.SpecificLocalAuthority,
            ProjectWorkflowState.LocalAuthorityConfirm => IsSiteNotIdentified() && _model.GeographicFocus is ProjectGeographicFocus.SpecificLocalAuthority,
            ProjectWorkflowState.LocalAuthorityNotFound => IsSiteNotIdentified() && _model.GeographicFocus is ProjectGeographicFocus.SpecificLocalAuthority,
            ProjectWorkflowState.HomesNumber => IsSiteNotIdentified(),
            ProjectWorkflowState.Progress => true,
            ProjectWorkflowState.RequiresFunding => true,
            ProjectWorkflowState.FundingAmount => IsFundingRequired(),
            ProjectWorkflowState.Profit => IsFundingRequired(),
            ProjectWorkflowState.ExpectedStart => true,
            ProjectWorkflowState.CheckAnswers => true,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(ProjectWorkflowState.EnglandHousingDelivery)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.Name, IsEligibleForAnything)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.NotEligibleForAnything, () => !IsEligibleForAnything());

        Machine.Configure(ProjectWorkflowState.NotEligibleForAnything)
            .Permit(Trigger.Back, ProjectWorkflowState.EnglandHousingDelivery);

        Machine.Configure(ProjectWorkflowState.Name)
            .Permit(Trigger.Continue, ProjectWorkflowState.SupportRequiredActivities)
            .Permit(Trigger.Back, ProjectWorkflowState.EnglandHousingDelivery);

        Machine.Configure(ProjectWorkflowState.SupportRequiredActivities)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.Infrastructure, () => IsActivityOnly(SupportActivityType.ProvidingInfrastructure))
            .PermitIf(Trigger.Continue, ProjectWorkflowState.Tenure, () => IsActivityOnly(SupportActivityType.DevelopingHomes))
            .PermitIf(
                Trigger.Continue,
                ProjectWorkflowState.IdentifiedSite,
                () => IsActivityNotOnly(SupportActivityType.ProvidingInfrastructure, SupportActivityType.DevelopingHomes))
            .Permit(Trigger.Back, ProjectWorkflowState.Name);

        Machine.Configure(ProjectWorkflowState.Infrastructure)
            .Permit(Trigger.Continue, ProjectWorkflowState.IdentifiedSite)
            .Permit(Trigger.Back, ProjectWorkflowState.SupportRequiredActivities);

        Machine.Configure(ProjectWorkflowState.Tenure)
            .Permit(Trigger.Continue, ProjectWorkflowState.OrganisationHomesBuilt)
            .Permit(Trigger.Back, ProjectWorkflowState.SupportRequiredActivities);

        Machine.Configure(ProjectWorkflowState.OrganisationHomesBuilt)
            .Permit(Trigger.Continue, ProjectWorkflowState.IdentifiedSite)
            .Permit(Trigger.Back, ProjectWorkflowState.Tenure);

        Machine.Configure(ProjectWorkflowState.IdentifiedSite)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.StartSiteDetails, IsSiteIdentified)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.GeographicFocus, IsSiteNotIdentified)
            .PermitIf(Trigger.Back, ProjectWorkflowState.Infrastructure, () => IsActivityOnly(SupportActivityType.ProvidingInfrastructure))
            .PermitIf(Trigger.Back, ProjectWorkflowState.OrganisationHomesBuilt, () => IsActivityOnly(SupportActivityType.DevelopingHomes))
            .PermitIf(
                Trigger.Back,
                ProjectWorkflowState.SupportRequiredActivities,
                () => IsActivityNotOnly(SupportActivityType.ProvidingInfrastructure, SupportActivityType.DevelopingHomes));

        Machine.Configure(ProjectWorkflowState.GeographicFocus)
            .PermitIf(
                Trigger.Continue,
                ProjectWorkflowState.HomesNumber,
                () => IsGeographicFocus(ProjectGeographicFocus.National, ProjectGeographicFocus.Unknown))
            .PermitIf(Trigger.Continue, ProjectWorkflowState.Region, () => IsGeographicFocus(ProjectGeographicFocus.Regional))
            .PermitIf(Trigger.Continue, ProjectWorkflowState.LocalAuthoritySearch, () => IsGeographicFocus(ProjectGeographicFocus.SpecificLocalAuthority))
            .Permit(Trigger.Back, ProjectWorkflowState.IdentifiedSite);

        Machine.Configure(ProjectWorkflowState.Region)
            .Permit(Trigger.Continue, ProjectWorkflowState.HomesNumber)
            .Permit(Trigger.Back, ProjectWorkflowState.GeographicFocus);

        Machine.Configure(ProjectWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Continue, ProjectWorkflowState.LocalAuthorityResult)
            .Permit(Trigger.Back, ProjectWorkflowState.GeographicFocus);

        Machine.Configure(ProjectWorkflowState.LocalAuthorityResult)
            .Permit(Trigger.Continue, ProjectWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Back, ProjectWorkflowState.LocalAuthoritySearch);

        Machine.Configure(ProjectWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, ProjectWorkflowState.Progress)
            .Permit(Trigger.Back, ProjectWorkflowState.LocalAuthorityResult);

        Machine.Configure(ProjectWorkflowState.LocalAuthorityNotFound)
            .Permit(Trigger.Back, ProjectWorkflowState.LocalAuthoritySearch);

        Machine.Configure(ProjectWorkflowState.HomesNumber)
            .Permit(Trigger.Continue, ProjectWorkflowState.Progress)
            .PermitIf(
                Trigger.Back,
                ProjectWorkflowState.GeographicFocus,
                () => IsGeographicFocus(ProjectGeographicFocus.National, ProjectGeographicFocus.Unknown))
            .PermitIf(Trigger.Back, ProjectWorkflowState.Region, () => IsGeographicFocus(ProjectGeographicFocus.Regional))
            .PermitIf(Trigger.Back, ProjectWorkflowState.LocalAuthoritySearch, () => IsGeographicFocus(ProjectGeographicFocus.SpecificLocalAuthority));

        Machine.Configure(ProjectWorkflowState.Progress)
            .Permit(Trigger.Continue, ProjectWorkflowState.RequiresFunding)
            .PermitIf(Trigger.Back, ProjectWorkflowState.LastSiteDetails, IsSiteIdentified)
            .PermitIf(Trigger.Back, ProjectWorkflowState.HomesNumber, IsSiteNotIdentified);

        Machine.Configure(ProjectWorkflowState.RequiresFunding)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.FundingAmount, IsFundingRequired)
            .PermitIf(Trigger.Continue, ProjectWorkflowState.ExpectedStart, () => !IsFundingRequired())
            .Permit(Trigger.Back, ProjectWorkflowState.Progress);

        Machine.Configure(ProjectWorkflowState.FundingAmount)
            .Permit(Trigger.Continue, ProjectWorkflowState.Profit)
            .Permit(Trigger.Back, ProjectWorkflowState.Progress);

        Machine.Configure(ProjectWorkflowState.Profit)
            .Permit(Trigger.Continue, ProjectWorkflowState.ExpectedStart)
            .Permit(Trigger.Back, ProjectWorkflowState.FundingAmount);

        Machine.Configure(ProjectWorkflowState.ExpectedStart)
            .Permit(Trigger.Continue, ProjectWorkflowState.CheckAnswers)
            .PermitIf(Trigger.Back, ProjectWorkflowState.RequiresFunding, () => !IsFundingRequired())
            .PermitIf(Trigger.Back, ProjectWorkflowState.Profit, IsFundingRequired);

        Machine.Configure(ProjectWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, ProjectWorkflowState.ExpectedStart);
    }

    private IEnumerable<Func<bool>> BuildDeadEndConditions(ProjectWorkflowState state)
    {
        if (state > ProjectWorkflowState.NotEligibleForAnything)
        {
            yield return IsEligibleForAnything;
        }
    }

    private bool IsEligibleForAnything() => _model.IsEnglandHousingDelivery;

    private bool IsActivityOnly(SupportActivityType supportActivity) => _model.SupportActivityTypes?.Count == 1
                                                          && _model.SupportActivityTypes.Single() == supportActivity;

    private bool IsActivityNotOnly(params SupportActivityType[] activity) => !(_model.SupportActivityTypes?.Count == 1
                                                                        && activity.Contains(_model.SupportActivityTypes.Single()));

    private bool IsSiteIdentified() => _model.IsSiteIdentified == true;

    private bool IsSiteNotIdentified() => _model.IsSiteIdentified == false;

    private bool IsFundingRequired() => _model.IsFundingRequired == true;

    private bool IsGeographicFocus(params ProjectGeographicFocus[] focus) => focus.Contains(_model.GeographicFocus);
}
