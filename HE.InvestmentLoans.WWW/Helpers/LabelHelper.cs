using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.WWW.Helpers
{
    public static class LabelHelper
    {
        public static string GetSummaryLabel(string value, string details)
        {
            if (string.IsNullOrEmpty(details))
                return value;

            return $"{value}, {details}";
        }

        public static string AdresAsHtml(AddressViewModel address) => address == null ? null :
            $"{address.Street}<br>" +
            $"{address.City}<br>" +
            $"{address.Postcode}<br>" +
            $"{address.Country}";
    }
}
