using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders
{
    internal class SiteViewModelObjectBuilder
    {
        private SiteViewModel _object;

        public SiteViewModelObjectBuilder()
        {
            _object = new SiteViewModel();
        }

        public static SiteViewModelObjectBuilder NewObject() {  return new SiteViewModelObjectBuilder(); }

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
                Ownership = "No"
            };

            return this;
        }

        public SiteViewModel Build()
        {
            return _object;
        }
    }
}
