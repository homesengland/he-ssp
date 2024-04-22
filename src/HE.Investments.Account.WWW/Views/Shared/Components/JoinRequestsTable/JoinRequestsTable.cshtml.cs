using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared;
using HE.Investments.Account.WWW.Controllers;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.JoinRequestsTable;

public class JoinRequestsTable : ViewComponent
{
    private readonly IAccountAccessContext _accountAccessContext;

    public JoinRequestsTable(IAccountAccessContext accountAccessContext)
    {
        _accountAccessContext = accountAccessContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(IList<UserDetails> users)
    {
        var canManageUsers = await _accountAccessContext.CanManageUsers();
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("First name"),
            new("Last name"),
            new("Email"),
            new("Job title"),
            new("Role"),
            new("Remove", IsHidden: true, IsDisplayed: canManageUsers),
            new("Manage", IsHidden: true, IsDisplayed: canManageUsers),
        };

        var usersDetails = users.Select(u =>
        {
            var tableItems = new List<TableValueViewModel>
            {
                new(u.FirstName),
                new(u.LastName),
                new(u.Email),
                new(u.JobTitle),
                new(u.Role?.GetDescription()),
                new(Component: CreateUsersLinkComponent(u.Id.Value, "Remove", nameof(UsersController.ConfirmUnlink)), IsDisplayed: canManageUsers),
                new(Component: CreateUsersLinkComponent(u.Id.Value, "Manage", nameof(UsersController.Manage)), IsDisplayed: canManageUsers),
            };

            return new TableRowViewModel(u.Id.Value, tableItems);
        }).ToList();

        return View("JoinRequestsTable", (tableHeaders, usersDetails));
    }

    private DynamicComponentViewModel CreateUsersLinkComponent(string id, string text, string action) =>
        new(nameof(Link), new { text, action, controller = new ControllerName(nameof(UsersController)).WithoutPrefix(), values = new { id } });
}
