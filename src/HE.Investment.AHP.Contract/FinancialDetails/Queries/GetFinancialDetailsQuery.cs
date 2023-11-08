using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Utils.Enums;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record GetFinancialDetailsQuery(string ApplicationId) : IRequest<FinancialDetails>;
