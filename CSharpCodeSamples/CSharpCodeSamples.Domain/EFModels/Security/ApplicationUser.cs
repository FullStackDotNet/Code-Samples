namespace CSharpCodeSamples.Domain.EFModels.Security
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser<int, IntranetUserLogin, IntranetUserRole, IntranetUserClaim>
    {
        [Required][StringLength(100)] public string FirstName     { get; set; }
        [Required][StringLength(100)] public string LastName      { get; set; }
        [Required][StringLength(100)] public string DefaultSearch { get; set; }
                                      public Int16  Theme         { get; set; }
    }
}
