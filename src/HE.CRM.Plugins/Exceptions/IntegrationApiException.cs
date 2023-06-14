using System;
using System.Text;

namespace HE.CRM.Plugins.Exceptions
{
    public class IntegrationApiException : Exception
    {
        public int HttpStatusCode { get; private set; }

        public IntegrationApiException(int httpStatusCode)
            : base()
        {
            HttpStatusCode = httpStatusCode;
        }

        public IntegrationApiException(int httpStatusCode, string message)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var separator = new string[] { Environment.NewLine };
            var str = base.ToString().Split(separator, 2, StringSplitOptions.None);
            sb.AppendLine(str[0]);
            sb.AppendLine($"HttpStatusCode: {HttpStatusCode}");
            sb.Append(str[1]);
            return sb.ToString();
        }
    }
}
