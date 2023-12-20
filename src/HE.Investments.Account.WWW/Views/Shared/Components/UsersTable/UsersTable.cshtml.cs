using HE.Investments.Account.Contract.Users;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Utils.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.UsersTable;

public class UsersTable : ViewComponent
{
    public IViewComponentResult Invoke(PaginationResult<UserDetails> users)
    {
        return View("UsersTable", users);
    }
}
