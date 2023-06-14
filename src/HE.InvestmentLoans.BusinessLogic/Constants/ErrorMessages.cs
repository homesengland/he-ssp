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
        public static ErrorMessages EstimatedPoundInput(string name) => PoundInput($"The estimated {name}");
        public static ErrorMessages AmountPoundInput(string name) => PoundInput($"The amount of {name} provided");
        public static ErrorMessages PoundInput(string name) => new ErrorMessages($"{name} must be entered as a number, in pounds and pence");
        public static ErrorMessages InvalidXYCoordinates(string invalidCharacters) => new ErrorMessages($"XY coordinates must not include {invalidCharacters}");

        public override string ToString()
        {
            return Value;
        }
    }
}
