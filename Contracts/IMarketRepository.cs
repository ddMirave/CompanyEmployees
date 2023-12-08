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
        IEnumerable<Market> GetAllMarkets(bool trackChanges);
        Market GetMarket(Guid marketId, bool trackChanges);
    }
}
