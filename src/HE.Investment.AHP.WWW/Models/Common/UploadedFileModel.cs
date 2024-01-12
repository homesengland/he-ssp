using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Common;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investment.AHP.WWW.Models.Common;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global", Justification = "Model used by JavaScript.")]
public record UploadedFileModel(string FileId, string FileName, string UploadDetails, bool CanBeRemoved)
{
    public static UploadedFileModel FromUploadedFile(UploadedFile file)
    {
        return new UploadedFileModel(
            file.FileId.Value,
            file.FileName,
            $"uploaded {DateHelper.DisplayAsUkFormatDateTime(file.UploadedOn)} by {file.UploadedBy}",
            file.CanBeRemoved);
    }
}
