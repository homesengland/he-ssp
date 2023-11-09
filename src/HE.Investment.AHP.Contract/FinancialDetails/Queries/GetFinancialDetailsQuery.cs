using HE.Investment.AHP.Contract.FinancialDetails.Models;
using MediatR;
using ApplicationId = HE.Investment.AHP.Contract.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record GetFinancialDetailsQuery(ApplicationId ApplicationId) : IRequest<FinancialDetailsModel>;
