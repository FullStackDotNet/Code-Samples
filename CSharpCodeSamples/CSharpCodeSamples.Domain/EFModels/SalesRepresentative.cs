namespace CSharpCodeSamples.Domain.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SalesRepresentative
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                                      public int       Id                         { get; set; }

                                      public bool      IsActive                   { get; set; }

        [Required][StringLength(100)] public string    FullName                  { get; set; }
                  [StringLength(25)]  public string    DisplayName                { get; set; }

                  [StringLength(40)]  public string    Position                   { get; set; }
                  [StringLength(25)]  public string    EmployeeIdentification     { get; set; }
                  [StringLength(10)]  public string    PhoneNumber                { get; set; }
                  [StringLength(8)]   public string    Extension                  { get; set; }
                  [StringLength(10)]  public string    CellNumber                 { get; set; }
                  [StringLength(10)]  public string    FaxNumber                  { get; set; }
                  [StringLength(50)]  public string    City                       { get; set; }
                  [StringLength(6)]   public string    State                      { get; set; }
                  [StringLength(5)]   public string    Region                     { get; set; }
                                      public decimal?  Longitude                  { get; set; }
                                      public decimal?  Latitude                   { get; set; }
                                      public DateTime  HireDate                   { get; set; }
                                      public DateTime? TermDate                   { get; set; }
                                      public string    ERPId                      { get; set; }
    }
}
