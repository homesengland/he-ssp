using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.Common.Utils.Constants;

public static class ValidationErrorMessage
{
    public const string EnterMoreDetails = "Enter more details";

    public const string InvalidValue = "Provided invalid value";

    public const string EnterMoreDetailsForRefinanceExitStrategy = "Enter more detail about your refinance exit strategy";

    public const string ManyHomesAmount = "The amount of homes must be a number containing no more than 4 digits (1 - 9999)";

    public const string TypeHomesOtherType = "Enter the type of home you are building";

    public const string CheckAnswersOption = "You have not completed this section. Select no if you want to come back later";

    public const string SecurityCheckAnswers = "Select whether you have completed this section";

    public const string NoStartDate = "Enter a build start date";

    public const string InvalidStartDate = "Enter a valid date. The build start date must include a day, month and year";

    public const string NoPurchaseDate = "Enter the date you purchased this site";

    public const string NoPurchaseDay = "The date you purchased this land must include a day";

    public const string NoPurchaseMonth = "The date you purchased this land must include a month";

    public const string NoPurchaseYear = "The date you purchased this land must include a year";

    public const string IncorrectPurchaseDate = "The date you purchased this land must be a real date";

    public const string FuturePurchaseDate = "The date you purchased this land must be today or in the past";

    public const string EnterExistingLegal = "Enter any existing legal charges or debt secured on this land";

    public const string HomesBuiltDecimalNumber = "The number of homes your organisation has built must be a whole number";

    public const string HomesBuiltIncorretInput = "The amount of homes your organisation has built must be a number";

    public const string HomesBuiltIncorrectNumber = "The number of homes your organisation has built in the past 3 years must be 99,999 or less";

    public const string FileIncorrectSize = "The selected file must be smaller than or equal to {0}MB";

    public const string FileIncorrectFormat = "The selected file must be a PDF, Word Doc, JPEG or RTF";

    public const string LoanPurpose = "Select what you need Homes England funding for";

    public const string EnterCoordinates = "Enter your XY coordinates";

    public const string EnterLandRegistryTitleNumber = "Enter your Land Registry title number";

    public const string IncorrectProjectCost = "The purchase value of the land must be entered as a number, in pounds and pence";

    public const string IncorrectProjectValue = "The current value of the land must be entered as a number, in pounds and pence";

    public const string IncorrectGrantFundingAmount = "The amount of funding provided";

    public const string EnterFirstName = "Enter your first name";

    public const string EnterLastName = "Enter your last name";

    public const string EnterJobTitle = "Enter your job title";

    public const string EnterTelephoneNumber = "Enter your preferred telephone number";

    public const string AcceptTermsAndConditions = "You cannot continue unless you have selected that you have read the terms and conditions";

    public const string DirectorLoansDoesNotExist = "Cannot add director loans subordinate because director loans does not exist.";

    public static string EstimatedPoundInput(string name) => PoundInput($"The estimated {name}");

    public static string PoundInput(string name) => new($"{name} must be entered as a number, in pounds and pence");

    public static string ShortInputLengthExcedeed(string fieldName) => new($"{fieldName.TitleCaseFirstLetterInString()} must be 100 characters or less ");

    public static string LongInputLengthExceeded(string fieldName) => new($"Request to change {fieldName} must be 1500 characters or less");
}
