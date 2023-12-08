using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MarketRepository : RepositoryBase<Market>, IMarketRepository
    {
        public MarketRepository(RepositoryContext repositoryContext): base(repositoryContext){}
        public async Task<IEnumerable<Market>> GetAllMarketsAsync(bool trackChanges) => await FindAll(trackChanges)
          .OrderBy(c => c.Name)
          .ToListAsync();
        public async Task<Market> GetMarketAsync(Guid marketId, bool trackChanges) => await FindByCondition(c => c.Id.Equals(marketId), trackChanges).SingleOrDefaultAsync();
        public void CreateMarket(Market market) => Create(market);
        public async Task<IEnumerable<Market>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        public void DeleteMarket(Market market)
        {
            Delete(market);
        }
    }
}
