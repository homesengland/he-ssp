using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using ApplicationId = HE.Investment.AHP.Contract.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record ProvidePurchasePriceCommand(ApplicationId ApplicationId, string PurchasePrice) : IRequest<OperationResult>;
