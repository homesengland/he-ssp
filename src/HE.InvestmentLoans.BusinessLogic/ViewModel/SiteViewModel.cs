using He.HelpToBuild.Apply.Application.Routing;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using Microsoft.Crm.Sdk.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel
{
    public class SiteViewModel
    {
        public SiteViewModel() {
            State = SiteWorkflow.State.Index;
            StateChanged = false;
        }

        public string CheckAnswers { get; set; }
        public string AffordableHomes { get; set; }
        public string PlanningRef { get; set; }
        public string PlanningRefEnter { get; set; }
        public string SitePurchaseFrom { get; set; }
        public string Ownership { get; set; }
        public string ManyHomes { get; set; }
        public string GrantFunding { get; set; }
        public string TitleNumber { get; set; }
        public string[] TypeHomes { get; set; }
        public string TypeHomesOther { get; set; }
        public string Cost { get; set; }
        public string Value { get; set; }
        public string HomesToBuild { get; set; }
        public string Source { get; set; }
        public string PurchaseDay { get; set; }
        public string PurchaseMonth { get; set; }
        public string PurchaseYear { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string LocationOption { get; set; }
        public string LocationCoordinates { get; set; }
        public string LocationLandRegistry { get; set; }
        public string Location { get; set; } = "Placeholder for API data";
        public string PlanningStatus { get; set; } = "Placeholder for API data";
        public string GrantFundingName { get; set; }
        public string GrantFundingSource { get; set; }
        public string GrantFundingAmount { get; set; }
        public string GrantFundingPurpose { get; set; }
        public string Type { get; set; }
        public string ChargesDebt { get; set; }
        public string ChargesDebtInfo { get; set; }
        public SiteWorkflow.State State { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public bool StateChanged { get; set; }
        public string HaveEstimatedStartDate{get;set;}
        public DateTime? EstimatedStartDate { get; set; }

        public void RemoveAlternativeRoutesData()
        {
            if(TypeHomes != null && !TypeHomes.Contains("other"))
            {
                TypeHomesOther = null;
            }

            if (PlanningRef == "No")
            {
                PlanningRefEnter = null;
                Location = null;
                PlanningStatus = null;
            }
            else if(PlanningRef == "Yes")
            {
                LocationCoordinates = null;
                LocationLandRegistry = null;
                LocationOption = null;
            }

            if(LocationOption == "coordinates")
            {
                LocationLandRegistry = null;
            }
            else if(LocationOption == "landRegistryTitleNumber")
            {
                LocationCoordinates = null;
            }

            if(Ownership == "No")
            {
                PurchaseDate = null;
                Cost = null;
                Value = null;
                Source = null;
            }

            if (GrantFunding != "Yes")
            {
                GrantFundingSource = null;
                GrantFundingAmount = null;
                GrantFundingName = null;
                GrantFundingPurpose = null;
            }

            if(ChargesDebt == "No")
            {
                ChargesDebtInfo = null;
            }
        }
    }
}
