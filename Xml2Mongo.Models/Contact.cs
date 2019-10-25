using System.Collections.Generic;

namespace Xml2Mongo.Models
{
    public class Contact : InventoryBase
    {
        public List<string> Phones { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
