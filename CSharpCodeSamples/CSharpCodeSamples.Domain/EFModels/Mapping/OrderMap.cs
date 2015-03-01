namespace CSharpCodeSamples.Domain.EFModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class OrderMap : EntityTypeConfiguration<Order>
    {
        //I have removed the details as they are proprietary
        //but you would use this to define your mappings
        public OrderMap()
        {
            ToTable("");  //table or view
            Property(t => t.Id).HasColumnName("Id");
            //Property mappings here
        }
    }
}
