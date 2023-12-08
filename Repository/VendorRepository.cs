using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
    {
        public VendorRepository(RepositoryContext repositoryContext) : base(repositoryContext){}
        public IEnumerable<Vendor> GetVendors(Guid marketId, bool trackChanges) => FindByCondition(e => e.MarketId.Equals(marketId), trackChanges)
            .OrderBy(e => e.Name);
        public Vendor GetVendor(Guid marketId, Guid id, bool trackChanges) => FindByCondition(e => e.MarketId.Equals(marketId) && e.Id.Equals(id), trackChanges).SingleOrDefault();
    }
}
