using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Security.Commands;
public record ProvideDirectorLoansSubordinateCommand(LoanApplicationId Id, string CanBeSubordinated, string ReasonWhyCannotBeSubordinated) : IRequest<OperationResult>;
