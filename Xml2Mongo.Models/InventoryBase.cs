using Xml2Mongo.SharedKernel;

namespace Xml2Mongo.Models
{
    public abstract class InventoryBase : Entity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
