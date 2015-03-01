namespace CSharpCodeSamples.Domain.EFModels.Security
{
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    public class IntranetUserStore : UserStore<ApplicationUser, IntranetRole, int, IntranetUserLogin, IntranetUserRole, IntranetUserClaim>
    {
        public IntranetUserStore(SecurityDbContext context) : base(context) {}

        public new virtual IQueryable<ApplicationUser> Users {
            get { return base.Users; }
        }
    }
}
