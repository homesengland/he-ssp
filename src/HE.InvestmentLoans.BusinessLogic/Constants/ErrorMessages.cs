using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

namespace HE.InvestmentLoans.BusinessLogic.Constants
{
    public class ErrorMessages
    {
        private ErrorMessages(string value) { Value = value; }

        public string Value { get; private set; }

        public static ErrorMessages RadioOption { get { return new ErrorMessages("Select one option"); } }
        public static ErrorMessages CheckboxOption { get { return new ErrorMessages("Select at least one option"); } }
        public static ErrorMessages EnterMoreDetails { get { return new ErrorMessages("Enter more details"); } }
        public static ErrorMessages InvalidReferenceNumber { get { return new ErrorMessages("Enter a valid planning reference number"); } }
        public static ErrorMessages ManyHomesAmount { get { return new ErrorMessages("The amount of homes must be a number containing no more than 4 digits (1 - 9999)"); } }
        public static ErrorMessages TypeHomesOtherType { get { return new ErrorMessages("Enter the type of home you are building"); } }
        public static ErrorMessages CheckAnswersOption { get { return new ErrorMessages("You have not completed this section. Select no if you want to come back later"); } }
        public static ErrorMessages SecurityCheckAnswers { get { return new ErrorMessages("Select whether you have completed this section"); } }
        public static ErrorMessages InvalidDate { get { return new ErrorMessages("Enter a valid date. The build start date must include a day, month and year"); } }
        public static ErrorMessages NoPurchaseDate { get { return new ErrorMessages("Enter the date you purchased this site"); } }
        public static ErrorMessages NoPurchaseDay { get { return new ErrorMessages("The date you purchased this land must include a day"); } }
        public static ErrorMessages NoPurchaseMonth { get { return new ErrorMessages("The date you purchased this land must include a month"); } }
        public static ErrorMessages NoPurchaseYear { get { return new ErrorMessages("The date you purchased this land must include a year"); } }
        public static ErrorMessages IncorrectPurchaseDate { get { return new ErrorMessages("The date you purchased this site land must be a real date"); } }
        public static ErrorMessages EstimatedPoundInput(string name) => PoundInput($"The estimated {name}");
        public static ErrorMessages AmountPoundInput(string name) => PoundInput($"The amount of {name} provided");
        public static ErrorMessages PoundInput(string name) => new ErrorMessages($"{name} must be entered as a number, in pounds and pence");
        public static ErrorMessages InvalidXYCoordinates(string invalidCharacters) => new ErrorMessages($"XY coordinates must not include {invalidCharacters}");
        public static ErrorMessages EnterExistingLegal
        {
            get
            {
                return new ErrorMessages($"Enter any existing legal charges or debt secured on this land");
            }
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
