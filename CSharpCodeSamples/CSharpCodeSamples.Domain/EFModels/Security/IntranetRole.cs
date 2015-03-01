namespace CSharpCodeSamples.Domain.EFModels.Security
{
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Provides a strongly typed int based Role
    /// </summary>
    public class IntranetRole : IdentityRole<int, IntranetUserRole>
    {
        public IntranetRole()
        {}
        public IntranetRole(string name)
        { Name = name; }
    }
}
