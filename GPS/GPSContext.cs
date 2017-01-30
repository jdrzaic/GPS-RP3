using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS
{
    class GPSGraph : Graph<GPSNode, GPSStreet> { }
    class GPSContext : DbContext
    {
        public DbSet<GPSGraph> Graphs { get; set; }
    }
}
