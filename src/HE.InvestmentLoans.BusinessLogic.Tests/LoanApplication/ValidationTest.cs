using FluentValidation;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication
{
    [TestClass]
    public class ValidationTest:MediatorTestBase
    {

        [TestMethod]
        [DataRow(nameof(LoanApplicationViewModel.Purpose))]
        public void Validate_Empty(string RequiredProperty)
        {

            LoanApplicationViewModel model = new LoanApplicationViewModel();
            var validator = serviceProvider.GetService<IValidator<LoanApplicationViewModel>>();
            FluentValidation.Results.ValidationResult results = new FluentValidation.Results.ValidationResult();
            foreach (var item in Enum.GetNames(typeof(LoanApplicationWorkflow.State)))
            {
                var validationResult = validator.Validate(model, opt => opt.IncludeRuleSets(item));
                if (!validationResult.IsValid)
                {
                    // error messages in the View.
                    results.Errors.AddRange(validationResult.Errors);
                    // re-render the view when validation failed.
                }
            }
            Assert.IsTrue(results.Errors.Any());
            Assert.IsTrue(results.Errors.Any(x => x.PropertyName == RequiredProperty));

        }


    }
}
