using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPS
{
    enum NodeType
    {
        Store,
        PostOffice,
        GasStation
    }

    [Table("GPSNode")]
    class GPSNode : ILocated
    {
        public PointF Location { get; set; }
        public Control AssociatedControl { get; set; }
        public NodeType NodeType { get; set; }
        public string Name { get; set; }
    }

    [Table("GPSStreet")]
    class GPSStreet
    {
        public Control AssociatedControl { get; set; }
        public string Name { get; set; }
    }
}
