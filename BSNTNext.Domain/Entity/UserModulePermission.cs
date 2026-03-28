namespace BSNTNext.Domain.Entity
{
    public class UserModulePermission
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }   

        public int ModulePermissionId { get; set; }
        public ModulePermission ModulePermission { get; set; } = null!;

        public Guid GrantedByUserId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    }
}