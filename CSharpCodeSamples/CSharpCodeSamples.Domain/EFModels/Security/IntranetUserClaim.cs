namespace CSharpCodeSamples.Domain.EFModels.Security
{
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Provides a strongly typed int based User Claim
    /// </summary>
    public class IntranetUserClaim : IdentityUserClaim<int>
    {}
}
