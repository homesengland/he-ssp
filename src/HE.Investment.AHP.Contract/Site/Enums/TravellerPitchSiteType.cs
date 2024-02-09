using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site.Enums;

public enum TravellerPitchSiteType
{
    Undefined = 0,

    [Description("Sites that are intended for permanent use as a traveller pitch site and provide pitches for long-term use by residents.")]
    Permanent,

    [Description("Sites that are intended for the permanent provision of transit pitches, providing temporary accommodation for travellers for up to 3 months.")]
    Transit,

    [Description("Sites that are only intended for temporary use as a traveller pitch site or which lack planning approval for permanent provision of traveller pitches.")]
    Temporary,
}
