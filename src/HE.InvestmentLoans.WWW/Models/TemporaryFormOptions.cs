using HE.InvestmentLoans.BusinessLogic.Enums;
using HE.InvestmentLoans.WWW.Helpers;
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

        public static List<SelectListItem> FundingPurposes { get; } = new List<SelectListItem>
        {
            SelectListHelper.FromEnum(FundingPurpose.BuildingNewHomes, "Building new homes"),
            SelectListHelper.FromEnum(FundingPurpose.BuildingInfrastructure, "Building infrastructure only"),
            SelectListHelper.FromEnum(FundingPurpose.Other, "Other")
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
                Value = "selfEstimate",
                Text = "Self estimate"
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
                Text = "Extra Care or assisted living"
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

        public static List<SelectListItem> HaveStartDate { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "Yes",
                Text = "Yes"
            },
            new SelectListItem()
            {
                Value = "No",
                Text = "No, I do not have a build start date yet"
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

        public static List<SelectListItem> PermissionStatus { get; } = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "notSubmitted",
                Text = "Application has not been submitted"
            },
            new SelectListItem()
            {
                Value = "notReceived",
                Text = "Not yet received result"
            },
            new SelectListItem()
            {
                Value = "outlineOrConsent",
                Text = "Received outline or reserved matters consent"
            },
            new SelectListItem()
            {
                Value = "receivedFull",
                Text = "Received full planning permission"
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
            "To find your project coordinates, use any online digital mapping service, such as Google Maps. " +
            "Find your project location and then right-click anywhere within the land boundary. This will provide the X/Y coordinates.",
            "You can find the title number on HM Land Registry documents, or correspondence with your solicitor when you purchased the land. " +
            "If you have multiple title numbers, separate these with a comma."
        };

        public static List<string> LocationDetailsNames { get; } = new()
        {
            "LocationCoordinates",
            "LocationLandRegistry"
        };
    }
}
