#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataverseModel
{
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_searchlocalauthority")]
	public partial class invln_searchlocalauthorityRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public const string ActionLogicalName = "invln_searchlocalauthority";
		
		public invln_searchlocalauthorityRequest()
		{
			this.RequestName = "invln_searchlocalauthority";
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_searchlocalauthority")]
	public partial class invln_searchlocalauthorityResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_localauthorities = "invln_localauthorities";
		}
		
		public const string ActionLogicalName = "invln_searchlocalauthority";
		
		public invln_searchlocalauthorityResponse()
		{
		}
		
		public string invln_localauthorities
		{
			get
			{
				if (this.Results.Contains("invln_localauthorities"))
				{
					return ((string)(this.Results["invln_localauthorities"]));
				}
				else
				{
					return default(string);
				}
			}
		}
	}
}
#pragma warning restore CS1591
