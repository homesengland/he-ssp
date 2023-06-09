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
	/// Status of the export
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum msdyn_exportstatus
	{
		
		/// <summary>
		/// The export has been completed.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		CompletedAllrecordsextracted = 192350002,
		
		/// <summary>
		/// The export has been performed but is incomplete. Some records will be missing.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Exportedwithmorerecordstoextract = 192350003,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Exporting = 192350001,
		
		/// <summary>
		/// The export request has been submitted.
		/// </summary>
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Submitted = 192350000,
	}
}
#pragma warning restore CS1591
