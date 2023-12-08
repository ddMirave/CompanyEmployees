using Contracts;
using Entities;
using Entities.Models;
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
        public IEnumerable<Market> GetAllMarkets(bool trackChanges) => FindAll(trackChanges)
          .OrderBy(c => c.Name)
          .ToList();
        public Market GetMarket(Guid marketId, bool trackChanges) => FindByCondition(c => c.Id.Equals(marketId), trackChanges).SingleOrDefault();
    }
}
