using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;

internal sealed class SiteViewModelObjectBuilder
{
    private SiteViewModel _object;

    public SiteViewModelObjectBuilder()
    {
        _object = new SiteViewModel();
    }

    public static SiteViewModelObjectBuilder NewObject() => new();

    public SiteViewModelObjectBuilder ThatPassesCheckAnswersValidation()
    {
        _object = new SiteViewModel
        {
            Name = "Test",
            ManyHomes = "12",
            HasEstimatedStartDate = "No",
            TypeHomes = new string[] { "tmp" },
            Type = "greenfield",
            AffordableHomes = "No",
            ChargesDebt = "No",
            HomesToBuild = "12",

            GrantFunding = "No",
            PlanningRef = "No",
            LocationOption = "coordinates",
            LocationCoordinates = "12,12 12,12",
            Ownership = "No",
        };

        return this;
    }

    public SiteViewModel Build()
    {
        return _object;
    }
}
