#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HE.Investments.Common.CRM.Model
{
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/crm/2011/Contracts")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("RetrieveEnvironmentVariables")]
	public partial class RetrieveEnvironmentVariablesRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public static class Fields
		{
			public const string SchemaNames = "SchemaNames";
		}
		
		public const string ActionLogicalName = "RetrieveEnvironmentVariables";
		
		public string[] SchemaNames
		{
			get
			{
				if (this.Parameters.Contains("SchemaNames"))
				{
					return ((string[])(this.Parameters["SchemaNames"]));
				}
				else
				{
					return default(string[]);
				}
			}
			set
			{
				this.Parameters["SchemaNames"] = value;
			}
		}
		
		public RetrieveEnvironmentVariablesRequest()
		{
			this.RequestName = "RetrieveEnvironmentVariables";
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/crm/2011/Contracts")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("RetrieveEnvironmentVariables")]
	public partial class RetrieveEnvironmentVariablesResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public static class Fields
		{
			public const string EntityCollection = "EntityCollection";
		}
		
		public const string ActionLogicalName = "RetrieveEnvironmentVariables";
		
		public RetrieveEnvironmentVariablesResponse()
		{
		}
		
		public Microsoft.Xrm.Sdk.EntityCollection EntityCollection
		{
			get
			{
				if (this.Results.Contains("EntityCollection"))
				{
					return ((Microsoft.Xrm.Sdk.EntityCollection)(this.Results["EntityCollection"]));
				}
				else
				{
					return default(Microsoft.Xrm.Sdk.EntityCollection);
				}
			}
		}
	}
}
#pragma warning restore CS1591
