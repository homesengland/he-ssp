﻿using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication
{
    public class LoanApplicationValidator : AbstractValidator<LoanApplicationViewModel>
    {
        public LoanApplicationValidator()
        {
            RuleSet("LoanPurpose", () =>
            {
                RuleFor(c => c.Purpose)
                    .NotEmpty()
                    .WithMessage("Select what you need Homes England funding for");
            });
        }
    }
}
