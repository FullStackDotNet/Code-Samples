namespace CSharpCodeSamples.Domain.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal  Id                       { get; set; }
        public string   OrderNumber              { get; set; }
        public DateTime OrderDate                { get; set; }
        public decimal  TotalAmount              { get; set; }
        public string   ShippingName             { get; set; }
        public string   SalesLocationId          { get; set; }
        public string   CustomerGroup            { get; set; }
        public string   SalesLocationName        { get; set; }
        public string   SalesRepId               { get; set; }
        public string   SalesRepFirstName        { get; set; }
        public string   SalesRepLastName         { get; set; }
    }
}
