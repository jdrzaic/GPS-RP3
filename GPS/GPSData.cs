using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPS
{
    public enum NodeType
    {
        Store,
        PostOffice,
        GasStation
    }

    [Table("GPSItemCharacteristic")]
    public class GPSCharacteristic
    {
        [Key]
        public int GPSCharacteristicId { get; set; }
        public NodeType NodeType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Table("GPSNode")]
    public class GPSNode : ILocated
    {
        [Key]
        public int GPSNodeId { get; set; }
        public PointF Location { get; set; }
        public Control AssociatedControl { get; set; }
        public IEnumerable<GPSCharacteristic> Characteristics { get; set; }
        public string Name { get; set; }
        public GPSNode()
        {
            Characteristics = new HashSet<GPSCharacteristic>();
        }
    }

    [Table("GPSStreet")]
    public class GPSStreet
    {
        [Key]
        public int GPSStreetId;
        public Control AssociatedControl { get; set; }
        public IEnumerable<GPSCharacteristic> Characteristics { get; set; }
        public string Name { get; set; }
        public GPSStreet()
        {
            Characteristics = new HashSet<GPSCharacteristic>();
        }
    }
}
