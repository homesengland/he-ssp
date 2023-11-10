using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Queries;
public record SearchLocalAuthoritiesQuery(string Phrase, int StartPage, int PageSize) : IRequest<OperationResult<LocalAuthoritiesViewModel>>;
