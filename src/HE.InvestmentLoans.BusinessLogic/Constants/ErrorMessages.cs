using System.Diagnostics.CodeAnalysis;

namespace HE.InvestmentLoans.BusinessLogic.Constants;

[SuppressMessage("Layout Rules", "SA1502", Justification = "It is bug for error messages. Allowed here but should be refactored.")]
public class ErrorMessages
{
    private ErrorMessages(string value) { Value = value; }

    public static ErrorMessages RadioOption => new("Select one option");

    public static ErrorMessages CheckboxOption => new("Select at least one option");

    public static ErrorMessages EnterMoreDetails => new("Enter more details");

    public static ErrorMessages InvalidReferenceNumber => new("Enter a valid planning reference number");

    public static ErrorMessages ManyHomesAmount => new("The amount of homes must be a number containing no more than 4 digits (1 - 9999)");

    public static ErrorMessages TypeHomesOtherType => new("Enter the type of home you are building");

    public static ErrorMessages CheckAnswersOption => new("You have not completed this section. Select no if you want to come back later");

    public static ErrorMessages SecurityCheckAnswers => new("Select whether you have completed this section");

    public static ErrorMessages NoStartDate => new("Enter a build start date");

    public static ErrorMessages InvalidStartDate => new("Enter a valid date. The build start date must include a day, month and year");

    public static ErrorMessages NoPurchaseDate => new("Enter the date you purchased this site");

    public static ErrorMessages NoPurchaseDay => new("The date you purchased this land must include a day");

    public static ErrorMessages NoPurchaseMonth => new("The date you purchased this land must include a month");

    public static ErrorMessages NoPurchaseYear => new("The date you purchased this land must include a year");

    public static ErrorMessages IncorrectPurchaseDate => new("The date you purchased this land must be a real date");

    public static ErrorMessages FuturePurchaseDate => new("The date you purchased this land must be today or in the past");

    public static ErrorMessages EnterExistingLegal => new($"Enter any existing legal charges or debt secured on this land");

    public static ErrorMessages HomesBuiltDecimalNumber => new("The number of homes your organisation has built must be a whole number");

    public static ErrorMessages HomesBuiltIncorretInput => new("The amount of homes your organisation has built must be a number");

    public static ErrorMessages HomesBuiltIncorrectNumber => new("The number of homes your organisation has built in the past 3 years must be 99,999 or less");

    public static ErrorMessages FileIncorrectSize => new("The selected file must be smaller than or equal to 20MB");

    public static ErrorMessages FileIncorrectFormat => new("The selected file must be a PDF, Word Doc, JPEG or RTF");

    public string Value { get; private set; }

    public static ErrorMessages EstimatedPoundInput(string name) => PoundInput($"The estimated {name}");

    public static ErrorMessages AmountPoundInput(string name) => PoundInput($"The amount of {name} provided");

    public static ErrorMessages PoundInput(string name) => new($"{name} must be entered as a number, in pounds and pence");

    public static ErrorMessages InvalidXYCoordinates(string invalidCharacters) => new($"XY coordinates must not include {invalidCharacters}");

    public override string ToString()
    {
        return Value;
    }
}
