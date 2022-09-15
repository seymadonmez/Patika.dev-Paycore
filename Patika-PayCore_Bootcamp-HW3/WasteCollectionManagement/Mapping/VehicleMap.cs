using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using WasteCollectionManagement.Models;

namespace WasteCollectionManagement.Mapping
{
    //Veritabanındaki Vehicke tablosu ile projedeki Vehicle classınin ilişkilendirilmesi
    public class VehicleMap: ClassMapping<Vehicle>
    {
        public VehicleMap()
        {
            Id(x => x.VehicleId, x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.Column("vehicleid");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            Property(v => v.VehicleName, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.Column("vehiclename");
                x.NotNullable(true);
            });
            Property(v => v.VehiclePlate, x =>
            {
                x.Length(14);
                x.Type(NHibernateUtil.String);
                x.Column("vehicleplate");
                x.NotNullable(true);
            });


            Table("vehicle");
        }
    }
}
