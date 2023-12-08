using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IVendorRepository
    {
        IEnumerable<Vendor> GetVendors(Guid marketId, bool trackChanges);
        Vendor GetVendor(Guid marketId, Guid id, bool trackChanges);
        void CreateVendorForMarket(Guid marketId, Vendor vendor);
        void DeleteVendor(Vendor vendor);

    }
}
