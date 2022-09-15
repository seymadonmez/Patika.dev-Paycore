using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using WasteCollectionManagement.Models;

namespace WasteCollectionManagement.Mapping
{
    //Veritabanındaki Container tablosu ile projedeki Container classınin ilişkilendirilmesi
    public class ContainerMap : ClassMapping<Container>
    {
        public ContainerMap()
        {
            Id(x => x.ContainerId, x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.Column("containerid");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            Property(c => c.ContainerName, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                //x.Column("containername");
            });
            Property(c => c.Latitude, x =>
            {
                //x.Length(10);
                x.Type(NHibernateUtil.Double);
                x.Column("latitude");
                x.Scale(0);
            });
            Property(c => c.Longitude, x =>
            {
                x.Length(10);
                x.Type(NHibernateUtil.Double);
                x.Column("longitude");
                x.Scale(0);
            });
            Property(c => c.VehicleId, x =>
            {
                x.Length(10);
                x.Type(NHibernateUtil.Int64);
                x.Column("vehicleid");
                x.Scale(0);
            });


            Table("container");
        }
    }
}
