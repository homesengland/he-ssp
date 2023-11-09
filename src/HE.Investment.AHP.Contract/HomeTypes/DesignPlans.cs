using HE.Investment.AHP.Contract.Common;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record DesignPlans(string? HomeTypeName, IList<HappiDesignPrincipleType> DesignPrinciples, string? MoreInformation, IList<UploadedFile> UploadedFiles);
