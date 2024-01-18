using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared;
using HE.Investments.Account.WWW.Controllers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.UsersTable;

public class UsersTable : ViewComponent
{
    private readonly IAccountAccessContext _accountAccessContext;

    public UsersTable(IAccountAccessContext accountAccessContext)
    {
        _accountAccessContext = accountAccessContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(PaginationResult<UserDetails> users)
    {
        var canManageUsers = await _accountAccessContext.CanManageUsers();
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("First name"),
            new("Last name"),
            new("Email"),
            new("Job title"),
            new("Role"),
            new("Last active"),
            new("Remove", IsHidden: true, IsDisplayed: canManageUsers),
            new("Manage", IsHidden: true, IsDisplayed: canManageUsers),
        };

        var usersDetails = users.Items.Select(u =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(u.FirstName),
                    new(u.LastName),
                    new(u.Email),
                    new(u.JobTitle),
                    new(u.Role?.GetDescription()),
                    new(u.LastActiveAt == null ? "-" : DateHelper.DisplayAsUkFormatDateTime(u.LastActiveAt)),
                    new(Component: CreateUsersLinkComponent(u.Id.Value, "Remove", nameof(UsersController.ConfirmUnlink)), IsDisplayed: canManageUsers),
                    new(Component: CreateUsersLinkComponent(u.Id.Value, "Manage", nameof(UsersController.Manage)), IsDisplayed: canManageUsers),
                };

                return new TableRowViewModel(tableItems);
            })
            .ToList();

        var rows = new PaginationResult<TableRowViewModel>(usersDetails, users.CurrentPage, users.ItemsPerPage, users.TotalItems);

        return View("UsersTable", (tableHeaders, rows));
    }

    private ComponentViewModel CreateUsersLinkComponent(string id, string text, string action) =>
        new(nameof(Link), new { text, action, controller = new ControllerName(nameof(UsersController)).WithoutPrefix(), values = new { id } });
}
