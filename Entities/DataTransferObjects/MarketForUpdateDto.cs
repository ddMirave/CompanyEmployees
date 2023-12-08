using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class MarketForUpdateDto
    {
        public string Name { get; set; }
        public string Placement { get; set; }
        public IEnumerable<VendorForCreationDto> Vendors { get; set; }
    }
}
