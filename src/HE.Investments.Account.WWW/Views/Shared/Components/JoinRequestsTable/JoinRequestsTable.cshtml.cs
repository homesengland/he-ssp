using HE.Investments.Account.Contract.Users;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.JoinRequestsTable;

public class JoinRequestsTable : ViewComponent
{
    public IViewComponentResult Invoke(IList<UserDetails> users)
    {
        return View("JoinRequestsTable", users);
    }
}
