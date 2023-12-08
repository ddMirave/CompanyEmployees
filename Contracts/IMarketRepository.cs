using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMarketRepository
    {
        Task<IEnumerable<Market>> GetAllMarketsAsync(bool trackChanges);
        Task<Market> GetMarketAsync(Guid marketId, bool trackChanges);
        void CreateMarket(Market market);
        Task<IEnumerable<Market>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteMarket(Market market);

    }
}
