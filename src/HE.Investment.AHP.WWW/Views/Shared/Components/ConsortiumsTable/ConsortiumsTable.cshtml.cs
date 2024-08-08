using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Enums;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Programme.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ConsortiumsTable;

public class ConsortiumsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(ConsortiumsList consortiumsList)
    {
        var organisationId = HttpContext.GetOrganisationIdFromRoute();
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Programme name", CellWidth.OneHalf),
            new("Lead Partner", CellWidth.OneThird),
            new("Membership status", CellWidth.OneEighth),
        };

        var rows = consortiumsList.Consortiums.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(organisationId, x.Programme, x.ConsortiumId)),
                    new(x.LeadPartnerName),
                    new(x.MembershipRole.GetDescription()),
                };

                return new TableRowViewModel(x.ConsortiumId.Value, tableItems);
            })
            .ToList();

        return Task.FromResult<IViewComponentResult>(View("ConsortiumsTable", (tableHeaders, rows)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(OrganisationId? organisationId, Programme programme, ConsortiumId consortiumId)
    {
        return new DynamicComponentViewModel(
            nameof(Link),
            new
            {
                text = programme.Name,
                controller = new ControllerName(nameof(ConsortiumMemberController)).WithoutPrefix(),
                action = nameof(ConsortiumMemberController.Index),
                values = new { consortiumId = consortiumId.Value, organisationId },
                isStrong = true,
            });
    }
}
