using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.InvestmentLoans.WWW.Models
{
    public class SelectItemWithSummaryLabel : SelectListItem
    {
        public string SummaryLabel { get; set; }
    }

    public static class TemporaryFormOptions
    {
        public static List<SelectItemWithSummaryLabel> LoanResolution { get; } = new List<SelectItemWithSummaryLabel>
        {
            new SelectItemWithSummaryLabel()
            {
                Value = "refinance",
                Text = "Refinance - you’re planning to build the homes and let them out, using an investment loan to refinance the proposed Homes England development loan",
                SummaryLabel = "Refinance"
            },
            new SelectItemWithSummaryLabel()
            {
                Value = "repay",
                Text = "Repay - you’re planning to build the homes and sell them when complete, using the sales income to repay the loan",
                SummaryLabel = "Repay"
            }
        };

        public static List<SelectListItem> FundingPurpose { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "buildingNewHomes",
                Text = "Building new homes "
            },
            new SelectListItem()
            {
                Value = "buildingInfrastructure",
                Text = "Building infrastructure only",
            },
            new SelectListItem()
            {
                Value = "other",
                Text = "Other",
            }
        };

        public static List<SelectListItem> FundingType { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "bank",
                Text = "Bank funding"
            },
            new SelectListItem()
            {
                Value = "grant",
                Text = "Grant funding"
            },
            new SelectListItem()
            {
                Value = "selfFunded",
                Text = "Self-funded"
            }
        };

        public static List<SelectListItem> SiteAdditionalSource { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "customerEstimate",
                Text = "Customer estimate"
            },
            new SelectListItem()
            {
                Value = "estateAgentEstimate",
                Text = "Estate agent estimate"
            },
            new SelectListItem()
            {
                Value = "ricsRedBookValuation",
                Text = "RICS Red Book valuation"
            }
        };

        public static List<SelectListItem> SiteTypeHomesCheckbox { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "apartmentsOrFlats",
                Text = "Apartments or flats"
            },
            new SelectListItem()
            {
                Value = "houses",
                Text = "Houses"
            },
            new SelectListItem()
            {
                Value = "bungalows",
                Text = "Bungalows"
            },
            new SelectListItem()
            {
                Value = "extraCareOrAssistedLiving",
                Text = "Extra care or assisted living"
            },
            new SelectListItem()
            {
                Value = "other",
                Text = "Other"
            }
        };

        public static List<SelectListItem> CheckAnswersCompletion { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "Yes",
                Text = "Yes, I`ve completed this section"
            },
            new SelectListItem()
            {
                Value = "No",
                Text = "No, I`ll come back later"
            }
        };

        public static List<SelectListItem> SiteType { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "Greenfield",
                Text = "Greenfield"
            },
            new SelectListItem()
            {
                Value = "Brownfield",
                Text = "Brownfield"
            }
        };

        public static List<SelectListItem> SiteLocation { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "coordinates",
                Text = "X/Y coordinates"
            },
            new SelectListItem()
            {
                Value = "landRegistryTitleNumber",
                Text = "Land Registry title number"
            }
        };

        public static List<SelectListItem> GrantFunding { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "Yes",
                Text = "Yes"
            },
            new SelectListItem()
            {
                Value = "No",
                Text = "No"
            },
            new SelectListItem()
            {
                Value = "doNotKnow",
                Text = "Do not know"
            }
        };

        public static List<string> LocationDescription { get; } = new()
        {
            "To find your site coordinates, use any online digital mapping service, such as Google Maps. " +
            "Find your site location and then right-click anywhere within the site boundary. This will provide the X/Y coordinates.",
            "Your site title number will begin with 2 or 3 letters to denote the area, followed by numbers. For example, NT123456. " +
            "This can be found on the Title Deeds, or correspondence with your solicitor when you purchased the site. If you have multiple title numbers, separate these with a comma."
        };

        public static List<string> LocationDetailsNames { get; } = new()
        {
            "locationCoordinates",
            "locationLandRegistry"
        };
    }
}
