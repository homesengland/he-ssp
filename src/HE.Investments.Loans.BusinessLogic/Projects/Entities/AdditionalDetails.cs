using HE.Investments.Loans.BusinessLogic.Projects.Enums;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.BusinessLogic.Projects.Entities;

public record AdditionalDetails(PurchaseDate PurchaseDate, Pounds Cost, Pounds CurrentValue, SourceOfValuation SourceOfValuation);
