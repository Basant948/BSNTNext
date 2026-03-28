namespace BSNTNext.Domain.Entity
{
    public class UserModuleAccess
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }  

        public int ModuleId { get; set; }
        public AppModule Module { get; set; } = null!;

        public Guid GrantedByUserId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    }
}