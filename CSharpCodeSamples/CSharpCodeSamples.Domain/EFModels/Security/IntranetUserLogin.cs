namespace CSharpCodeSamples.Domain.EFModels.Security
{
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Provides a strongly typed int based User Login
    /// </summary>
    public class IntranetUserLogin : IdentityUserLogin<int>
    {}
}
