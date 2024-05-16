namespace HE.Investment.AHP.WWW.Views.Shared.Components.ProjectListCard;

public record ProjectListCardModel(
    string Header,
    string HeaderLinkUrl,
    IList<ProjectListCardItemModel>? Items,
    string ViewAllUrl);
