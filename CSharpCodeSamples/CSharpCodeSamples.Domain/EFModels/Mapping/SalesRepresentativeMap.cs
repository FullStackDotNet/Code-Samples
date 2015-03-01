namespace CSharpCodeSamples.Domain.EFModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class SalesRepresentativeMap : EntityTypeConfiguration<SalesRepresentative>
    {
        //I have removed the details as they are proprietary
        //but you would use this to define your mappings
        public SalesRepresentativeMap()
        {
            ToTable("");
            Property(t => t.Id).HasColumnName("Id");
            //Property mappings here
        }
    }
}
