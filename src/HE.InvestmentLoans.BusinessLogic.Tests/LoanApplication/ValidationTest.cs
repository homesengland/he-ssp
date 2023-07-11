using FluentValidation;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication;

[TestClass]
public class ValidationTest : MediatorTestBase
{
    [TestMethod]
    [DataRow(nameof(LoanApplicationViewModel.Purpose))]
    public void Validate_Empty(string requiredProperty)
    {
        var model = new LoanApplicationViewModel();
        var validator = ServiceProvider.GetService<IValidator<LoanApplicationViewModel>>();
        var results = new FluentValidation.Results.ValidationResult();
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
        Assert.IsTrue(results.Errors.Any(x => x.PropertyName == requiredProperty));
    }
}
