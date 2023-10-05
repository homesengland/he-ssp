using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideGrantFundingInformationCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ProviderName, string Amount, string Name, string Purpose) : IRequest<OperationResult>;
