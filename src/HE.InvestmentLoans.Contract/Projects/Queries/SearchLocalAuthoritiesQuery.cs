using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Queries;
public record SearchLocalAuthoritiesQuery(string Phrase, int StartPage, int PageSize) : IRequest<OperationResult<LocalAuthoritiesViewModel>>;
