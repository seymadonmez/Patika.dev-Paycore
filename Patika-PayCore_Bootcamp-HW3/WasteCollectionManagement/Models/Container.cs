namespace WasteCollectionManagement.Models
{
    public class Container
    {
        public virtual long ContainerId { get; set; }
        public virtual string ContainerName { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual long VehicleId  { get; set; }

    }
}
