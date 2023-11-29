namespace HE.Investments.Common.Messages;

public static class GenericValidationError
{
    public const string NoDate = "Provide date";

    public const string InvalidDate = "Enter valid date";

    public const string InvalidPoundsValue = "Invalid pounds value";

    public const string TextTooLong = "Text exceeds limit";

    public const string NoValueProvided = "Value is not provided";

    public const string FileUniqueName = "File with that name is already uploaded.";

    public static string InvalidFileType(string fileName, IEnumerable<string> allowedExtensions) => $"The selected file {fileName} must be a {FileExtensions(allowedExtensions)}";

    public static string FileTooBig(int maxSizeInMb) => $"The selected file must be smaller than {maxSizeInMb}MB";

    public static string FileCountLimit(int numberOfFiles) => $"You can only select up to {numberOfFiles} files";

    private static string FileExtensions(IEnumerable<string> extensions)
    {
        var upperExtensions = extensions.Select(x => x.ToUpperInvariant()).ToArray();
        return upperExtensions.Length switch
        {
            0 => throw new InvalidOperationException("Cannot generate Validation Error message because there are no file Extensions."),
            1 => upperExtensions[0],
            _ => $"{string.Join(", ", upperExtensions[..^1])} or {upperExtensions[^1]}",
        };
    }
}
