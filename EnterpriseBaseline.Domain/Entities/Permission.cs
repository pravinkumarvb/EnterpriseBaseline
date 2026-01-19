using EnterpriseBaseline.Domain.Entities.Base;

namespace EnterpriseBaseline.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Code { get; set; } = null!; // e.g. Users.Create
        public string Description { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
