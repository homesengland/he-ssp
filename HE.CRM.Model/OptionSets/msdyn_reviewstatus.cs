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
	
	
	/// <summary>
	/// Whether or not an automatically-generated attribute value or attribute values has been approved by your organanization's admin.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum msdyn_reviewstatus
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Approved = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Dismissed = 2,
		
		/// <summary>
		/// This means that the entity has not been reviewed by user
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Notapproved = 0,
	}
}
#pragma warning restore CS1591
