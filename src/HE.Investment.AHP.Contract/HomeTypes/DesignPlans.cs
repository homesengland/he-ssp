using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record DesignPlans(string? HomeTypeName, IList<HappiDesignPrincipleType> DesignPrinciples, string? MoreInformation, IList<UploadedFile> UploadedFiles);
