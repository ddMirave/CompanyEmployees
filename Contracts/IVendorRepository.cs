using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IVendorRepository
    {
        Task<PagedList<Vendor>> GetVendorsAsync(Guid marketId, VendorParameters vendorParameters, bool trackChanges);
        Task<Vendor> GetVendorAsync(Guid marketId, Guid id, bool trackChanges);
        void CreateVendorForMarket(Guid marketId, Vendor vendor);
        void DeleteVendor(Vendor vendor);

    }
}
