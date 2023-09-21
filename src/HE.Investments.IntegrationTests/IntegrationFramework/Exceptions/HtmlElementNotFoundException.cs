using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Exceptions;
public class HtmlElementNotFoundException : Exception
{
    public HtmlElementNotFoundException(string? message)
        : base(message)
    {
    }
}
