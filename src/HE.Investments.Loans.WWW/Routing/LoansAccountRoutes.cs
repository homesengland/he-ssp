using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.WWW.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Routing;

public class LoansAccountRoutes : IAccountRoutes
{
    public IActionResult NotCompleteProfile()
    {
        return new RedirectToActionResult(
            nameof(UserController.ProfileDetails),
            new ControllerName(nameof(UserController)).WithoutPrefix(),
            null);
    }

    public IActionResult NotLinkedOrganisation()
    {
        return new RedirectToActionResult(
            nameof(OrganizationController.SearchOrganization),
            new ControllerName(nameof(OrganizationController)).WithoutPrefix(),
            null);
    }

    public IActionResult LandingPageForNotLoggedUser()
    {
        return new RedirectToActionResult(
            nameof(GuidanceController.WhatTheHomeBuildingFundIs),
            new ControllerName(nameof(GuidanceController)).WithoutPrefix(),
            null);
    }

    public IActionResult LandingPageForLoggedUser()
    {
        return new RedirectToActionResult(
            nameof(UserOrganisationController.Index),
            new ControllerName(nameof(UserOrganisationController)).WithoutPrefix(),
            null);
    }
}
