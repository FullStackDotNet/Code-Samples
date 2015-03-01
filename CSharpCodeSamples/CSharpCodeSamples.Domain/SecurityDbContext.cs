namespace CSharpCodeSamples.Domain
{
    using Microsoft.AspNet.Identity.EntityFramework;

    using EFModels.Security;

    public class SecurityDbContext : IdentityDbContext<ApplicationUser, IntranetRole, int, IntranetUserLogin, IntranetUserRole, IntranetUserClaim>
    {
        public SecurityDbContext()
            : base("name=ApplicationServices")
        { }

        public static SecurityDbContext Create()
        {
            return new SecurityDbContext();
        }
    }
}
