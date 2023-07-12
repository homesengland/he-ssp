namespace HE.Xrm.ServiceClientExample.Model.EntitiesDto
{
    public class ContactRolesDto
    {
        public List<ContactRoleDto> contactRoles { get; set; }
        public string externalId { get; set; }
        public string email { get; set; }
    }

    public class ContactRoleDto
    {
        public string accountName { get; set; }
        public Guid accountId { get; set; }
        public string webRoleName { get; set; }
        public string permissionLevel { get; set; }
    }
}
