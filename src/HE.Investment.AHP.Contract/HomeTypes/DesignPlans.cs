namespace HE.Investment.AHP.Contract.HomeTypes;

public record DesignPlans(string? HomeTypeName, IList<HappiDesignPrincipleType> DesignPrinciples);
