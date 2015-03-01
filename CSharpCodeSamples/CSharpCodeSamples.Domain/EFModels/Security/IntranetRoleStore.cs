namespace CSharpCodeSamples.Domain.EFModels.Security
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class IntranetRoleStore : RoleStore<IntranetRole, int, IntranetUserRole>
    {
        public IntranetRoleStore(SecurityDbContext context) : base(context) {}
    }
}
