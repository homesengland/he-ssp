using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

[Route("guidance")]
public class GuidanceController : Controller
{
    [HttpGet("what-the-home-building-fund-is")]
    public IActionResult WhatTheHomeBuildingFundIs()
    {
        return View();
    }

    [HttpGet("eligibility")]
    public IActionResult Eligibility()
    {
        return View();
    }

    [HttpGet("apply")]
    public IActionResult Apply()
    {
        return View();
    }

    [HttpGet("what-happens-after-apply")]
    public IActionResult AfterApply()
    {
        return View();
    }

    [HttpGet("how-we-prioritise-applications")]
    public IActionResult PrioritiseApplications()
    {
        return View();
    }
}
