using HE.Investments.Common.WWW.Components;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record ListCardItemModel(string Name, DynamicComponentViewModel StatusComponent, string Url);
