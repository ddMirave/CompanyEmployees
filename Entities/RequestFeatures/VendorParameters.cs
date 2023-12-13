using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class VendorParameters : RequestParameters
    {
        public VendorParameters()
        {
            OrderBy = "name";
        }
        public string SearchTerm { get; set; }
    }
}
