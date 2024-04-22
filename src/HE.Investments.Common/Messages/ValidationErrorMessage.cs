using System.Globalization;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.Messages;

public static class ValidationErrorMessage
{
    public const string EnterMoreDetails = "Enter more details";

    public const string EnterUkTelephoneNumber = "Enter a UK telephone number";

    public const string EnterTelephoneNumberInValidFormat = "Enter a telephone number, like 01632 960 001, 07700 900 982 or +44 808 157 0192";

    public const string EnterWhyYouWantToWithdrawApplication = "Enter why you want to withdraw your application";

    public const string InvalidValue = "Provided invalid value";

    public const string ChooseYourAnswer = "Choose your answer";

    public const string ManyHomesAmount = "The amount of homes must be a number containing no more than 4 digits (1 - 9999)";

    public const string TypeHomesOtherType = "Enter the type of home you are building";

    public const string CheckAnswersOption = "You have not completed this section. Select no if you want to come back later";

    public const string NoCheckAnswers = "Select whether you have completed this section";

    public const string FuturePurchaseDate = "The date you purchased this land must be today or in the past";

    public const string EnterExistingLegal = "Enter any existing legal charges or debt secured on this land";

    public const string HomesBuiltDecimalNumber = "The number of homes your organisation has built must be a whole number";

    public const string HomesBuiltIncorretInput = "The amount of homes your organisation has built must be a number";

    public const string HomesBuiltIncorrectNumber = "The number of homes your organisation has built in the past 3 years must be 99,999 or less";

    public const string FileIncorrectSize = "The selected file must be smaller than {0}MB";

    public const string FileIncorrectFormat = "The selected file must be a PDF, Word Doc, JPEG or RTF";

    public const string FileFormatNotSupported = "The selected file type is not supported";

    public const string LoanPurpose = "Select what you need Homes England funding for";

    public const string EnterCoordinates = "Enter your XY coordinates";

    public const string EnterLandRegistryTitleNumber = "Enter your Land Registry title number";

    public const string IncorrectProjectCost = "The purchase value of the land must be entered as a number, in pounds and pence";

    public const string IncorrectProjectValue = "The current value of the land must be entered as a number, in pounds and pence";

    public const string AcceptTermsAndConditions = "Select that you have read and understood the privacy notice";

    public const string AcceptTermsAndConditionsAndContinue = "You must select you have read and understood the privacy policy to continue";

    public const string InformationAgreement = "Select that you have read and agree with the information";

    public const string DirectorLoansDoesNotExist = "Cannot add director loans subordinate because director loans does not exist.";

    public const string ProjectNameIsEmpty = "Project name cannot be empty";

    public const string LocalAuthorityNameIsEmpty = "Enter the name of the local authority";

    public const string SectionIsNotCompleted = "You have not completed this section. Select no if you want to come back later";

    public const string CouldNotCalculate = "Could not calculate as all fields have not been entered";

    public const string EnterDate = "Enter a date. The date must include a day, month and year";

    public const string FilesListEmpty = "Select at least one file to upload";

    public const string ProvideAllProjectAnswers = "To submit, you have to provide all answers";

    public const string ProvideAllSiteAnswers = "To submit, you have to provide all answers for sites";

    public static string FilesMaxCount(int numberOfFiles) => $"You can only select up to {numberOfFiles} files";

    public static string EstimatedPoundInput(string name) => PoundInput($"estimated {name}");

    public static string PercentageInput(string name) => new($"The {name.ToLowerInvariant()} must be entered as a whole percentage value, like 30");

    public static string PoundInput(string name) => new($"The {name} must be entered as a number, in pounds and pence");

    public static string ShortInputLengthExceeded(string fieldName) => new($"The {fieldName} must be 100 characters or less");

    public static string LongInputLengthExceeded(string fieldName) => new($"The {fieldName} must be 1500 characters or less");

    public static string MissingRequiredField(string displayName) => $"Enter {displayName}";

    public static string MustProvideRequiredField(string displayName) => $"Enter the {displayName}";

    public static string MustProvideYourRequiredField(string displayName) => $"Enter your {displayName}";

    public static string MustProvideYourHigherNumber<TNumber>(string fieldName, TNumber minValue) => string.Format(CultureInfo.InvariantCulture, "Your {0} must be {1} or more", fieldName, minValue);

    public static string MustProvideYourLowerNumber<TNumber>(string fieldName, TNumber maxValue) => string.Format(CultureInfo.InvariantCulture, "Your {0} must be {1} or fewer", fieldName, maxValue);

    public static string MustProvideTheHigherNumber<TNumber>(string fieldName, TNumber minValue) => string.Format(CultureInfo.InvariantCulture, "The {0} must be {1} or more", fieldName, minValue);

    public static string MustProvideTheLowerNumber<TNumber>(string fieldName, TNumber maxValue) => string.Format(CultureInfo.InvariantCulture, "The {0} must be {1} or fewer", fieldName, maxValue);

    public static string MustBeYourWholeNumber(string fieldName, string? example) => $"Your {fieldName} must be a whole number{Example(example)}";

    public static string MustBeTheWholeNumber(string displayName, string? example) => $"The {displayName} must be a whole number{Example(example)}";

    public static string MustNotIncludeYourPence(string displayName, string? example) => $"Your {displayName} must not include pence{Example(example)}";

    public static string MustNotIncludeThePence(string displayName, string? example) => $"The {displayName} must not include pence{Example(example)}";

    public static string MustIncludeThePence(string displayName, string? example) => $"The {displayName} must include pence{Example(example)}";

    public static string MustIncludeThePrecision(string displayName, int precision, string? example) => $"The {displayName} must include {precision} decimal places{Example(example)}";

    public static string MustBeTheNumber(string displayName, string? example) => $"The {displayName} must be a number{Example(example)}";

    public static string MustBeProvidedForCalculation(string displayName) => $"Enter the {displayName} to calculate";

    public static string MustBeSelectedForCalculation(string displayName) => $"Select if {displayName} to calculate";

    public static string StringLengthExceeded(string displayName, int maxLength) => new($"The {displayName} must be {maxLength} characters or less");

    public static string YourStringLengthExceeded(string displayName, int maxLength) => new($"Your {displayName} must be {maxLength} characters or less");

    public static string StringLengthExceededUncommon(string displayName, int maxLength) => new($"{displayName.TitleCaseFirstLetterInString()} must be {maxLength} characters or less");

    public static string ExclusiveOptionSelected(string displayName, string optionName) => $"The {optionName} {displayName} option is exclusive and cannot be selected with any other option";

    public static string MustBeDate(string displayName) => $"The {displayName} must be a valid date";

    public static string MustBeSelected(string displayName) => $"Select the {displayName}";

    private static string Example(string? example)
    {
        return example.IsProvided() ? $", like {example}" : string.Empty;
    }
}
