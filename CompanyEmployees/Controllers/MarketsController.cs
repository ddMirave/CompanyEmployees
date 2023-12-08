using AutoMapper;
using CompanyEmployees.ModelBinders;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/markets")]
    [ApiController]
    public class MarketsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public MarketsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetMarkets()
        {
            var markets = _repository.Market.GetAllMarkets(trackChanges: false);
            var marketsDto = _mapper.Map<IEnumerable<MarketDto>>(markets);
            return Ok(marketsDto);
        }
        [HttpGet("{id}", Name = "MarketById")]
        public IActionResult GetMarket(Guid id)
        {
            var market = _repository.Market.GetMarket(id, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var marketDto = _mapper.Map<MarketDto>(market);
                return Ok(marketDto);
            }
        }
        [HttpPost]
        public IActionResult CreateMarket([FromBody] MarketForCreationDto market)
        {
            if (market == null)
            {
                _logger.LogError("MarketForCreationDto object sent from client is null.");
                return BadRequest("MarketForCreationDto object is null");
            }
            var marketEntity = _mapper.Map<Market>(market);
            _repository.Market.CreateMarket(marketEntity);
            _repository.Save();
            var marketToReturn = _mapper.Map<MarketDto>(marketEntity);
            return CreatedAtRoute("MarketById", new { id = marketToReturn.Id }, marketToReturn);
        }
        [HttpGet("collection/({ids})", Name = "MarketCollection")]
        public IActionResult GetMarketCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var marketEntities = _repository.Market.GetByIds(ids, trackChanges: false);
            if (ids.Count() != marketEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var marketsToReturn = _mapper.Map<IEnumerable<MarketDto>>(marketEntities);
            return Ok(marketsToReturn);
        }
        [HttpPost("collection")]
        public IActionResult CreateMarketCollection([FromBody] IEnumerable<MarketForCreationDto> marketCollection)
        {
            if (marketCollection == null)
            {
                _logger.LogError("Market collection sent from client is null.");
                return BadRequest("Market collection is null");
            }
            var marketEntities = _mapper.Map<IEnumerable<Market>>(marketCollection);
            foreach (var market in marketEntities)
            {
                _repository.Market.CreateMarket(market);
            }
            _repository.Save();
            var marketCollectionToReturn = _mapper.Map<IEnumerable<MarketDto>>(marketEntities);
            var ids = string.Join(",", marketCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("MarketCollection", new { ids }, marketCollectionToReturn);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteMarket(Guid id)
        {
            var market = _repository.Market.GetMarket(id, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Market.DeleteMarket(market);
            _repository.Save();
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMarket(Guid id, [FromBody] MarketForUpdateDto market)
        {
            if (market == null)
            {
                _logger.LogError("MarketForUpdateDto object sent from client is null.");
                return BadRequest("MarketForUpdateDto object is null");
            }
            var marketEntity = _repository.Market.GetMarket(id, trackChanges: true);
            if (marketEntity == null)
            {
                _logger.LogInfo($"Market with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(market, marketEntity);
            _repository.Save();
            return NoContent();
        }
    }
}
