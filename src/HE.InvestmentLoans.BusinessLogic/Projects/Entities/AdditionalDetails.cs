using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Common;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Entities;
public record AdditionalDetails(PurchaseDate PurchaseDate, Pounds Cost, Pounds CurrentValue, SourceOfValuation SourceOfValuation);
