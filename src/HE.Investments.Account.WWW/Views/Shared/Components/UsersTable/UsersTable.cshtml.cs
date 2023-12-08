using HE.Investments.Account.Contract.Users;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.UsersTable;

public class UsersTable : ViewComponent
{
    public IViewComponentResult Invoke(IList<UserDetails> users)
    {
        return View("UsersTable", users);
    }
}
