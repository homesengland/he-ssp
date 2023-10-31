﻿using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideAffordableHomesCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string AffordableHomes) : IRequest<OperationResult>;
