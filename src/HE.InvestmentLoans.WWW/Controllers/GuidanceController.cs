using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

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
}
