#pragma warning disable CS1591
// Code Generated by DLaB.ModelBuilderExtensions
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
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("invln_savetemporaryportaldata")]
	public partial class invln_savetemporaryportaldataRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string invln_dataid = "invln_dataid";
			public const string invln_databody = "invln_databody";
		}
		
		public const string ActionLogicalName = "invln_savetemporaryportaldata";
		
		public string invln_dataid
		{
			get
			{
				if (this.Parameters.Contains("invln_dataid"))
				{
					return ((string)(this.Parameters["invln_dataid"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_dataid"] = value;
			}
		}
		
		public string invln_databody
		{
			get
			{
				if (this.Parameters.Contains("invln_databody"))
				{
					return ((string)(this.Parameters["invln_databody"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["invln_databody"] = value;
			}
		}
		
		public invln_savetemporaryportaldataRequest()
		{
			this.RequestName = "invln_savetemporaryportaldata";
			this.invln_dataid = default(string);
			this.invln_databody = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("invln_savetemporaryportaldata")]
	public partial class invln_savetemporaryportaldataResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string invln_databody = "invln_databody";
		}
		
		public const string ActionLogicalName = "invln_savetemporaryportaldata";
		
		public invln_savetemporaryportaldataResponse()
		{
		}
		
		public string invln_databody
		{
			get
			{
				if (this.Results.Contains("invln_databody"))
				{
					return ((string)(this.Results["invln_databody"]));
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
