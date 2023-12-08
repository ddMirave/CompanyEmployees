using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
    {
        public VendorRepository(RepositoryContext repositoryContext) : base(repositoryContext){}
        public async Task<IEnumerable<Vendor>> GetVendorsAsync(Guid marketId, bool trackChanges) => await FindByCondition(e => e.MarketId.Equals(marketId), trackChanges)
            .OrderBy(e => e.Name)
            .ToListAsync();
        public async Task<Vendor> GetVendorAsync(Guid marketId, Guid id, bool trackChanges) => await FindByCondition(e => e.MarketId.Equals(marketId) && e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void CreateVendorForMarket(Guid marketId, Vendor vendor)
        {
            vendor.MarketId = marketId;
            Create(vendor);
        }
        public void DeleteVendor(Vendor vendor)
        {
            Delete(vendor);
        }
    }
}
