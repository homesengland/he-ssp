using System.ComponentModel;

namespace HE.Investments.FrontDoor.Shared.Project.Contract;

public enum RequiredFundingOption
{
    Undefined = 0,

    [Description("Less than \u00a3250,000")]
    LessThan250K = 1,

    [Description("\u00a3250,000 to \u00a31 million")]
    Between250KAnd1Mln,

    [Description("\u00a31 million to \u00a35 million")]
    Between1MlnAnd5Mln,

    [Description("\u00a35 million to \u00a310 million")]
    Between5MlnAnd10Mln,

    [Description("\u00a310 million to \u00a330 million")]
    Between10MlnAnd30Mln,

    [Description("\u00a330 million to \u00a350 million")]
    Between30MlnAnd50Mln,

    [Description("More than \u00a350 million")]
    MoreThan50Mln,

    [Description("I do not know")]
    IDoNotKnow,
}
