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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_updateuserprofile")]
	public partial class invln_updateuserprofileRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_contactexternalid = "invln_contactexternalid";
			public const string invln_contact = "invln_contact";
		}
		
		public const string ActionLogicalName = "invln_updateuserprofile";
		
		public string invln_contactexternalid
		{
			get
			{
				if (this.Parameters.Contains("invln_contactexternalid"))
				{
					return ((string)(this.Parameters["invln_contactexternalid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_contactexternalid"] = value;
			}
		}
		
		public string invln_contact
		{
			get
			{
				if (this.Parameters.Contains("invln_contact"))
				{
					return ((string)(this.Parameters["invln_contact"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_contact"] = value;
			}
		}
		
		public invln_updateuserprofileRequest()
		{
			this.RequestName = "invln_updateuserprofile";
			this.invln_contactexternalid = default(string);
			this.invln_contact = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_updateuserprofile")]
	public partial class invln_updateuserprofileResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public const string ActionLogicalName = "invln_updateuserprofile";
		
		public invln_updateuserprofileResponse()
		{
		}
	}
}
#pragma warning restore CS1591
