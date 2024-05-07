using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ConsortiumsTable;

public class ConsortiumsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(ConsortiumsList consortiumsList)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Consortium name", CellWidth.OneHalf),
            new("Lead Partner", CellWidth.OneThird),
            new("Membership status", CellWidth.OneEighth),
        };

        var rows = consortiumsList.Consortiums.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(x.Programme, x.ConsortiumId)),
                    new(x.LeadPartnerName),
                    new(x.MembershipRole.GetDescription()),
                };

                return new TableRowViewModel(x.ConsortiumId.Value, tableItems);
            })
            .ToList();

        return Task.FromResult<IViewComponentResult>(View("ConsortiumsTable", (tableHeaders, rows)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(ProgrammeSlim programme, ConsortiumId consortiumId)
    {
        return new DynamicComponentViewModel(
            nameof(Link),
            new
            {
                text = programme.Name,
                controller = new ControllerName(nameof(ConsortiumMemberController)).WithoutPrefix(),
                action = nameof(ConsortiumMemberController.Index),
                values = new { consortiumId = consortiumId.Value },
                isStrong = true,
            });
    }
}
