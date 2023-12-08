using AutoMapper;
using CompanyEmployees.ActionFilters;
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
        public async Task<IActionResult> GetMarkets()
        {
            var markets = await _repository.Market.GetAllMarketsAsync(trackChanges: false);
            var marketsDto = _mapper.Map<IEnumerable<MarketDto>>(markets);
            return Ok(marketsDto);
        }
        [HttpGet("{id}", Name = "MarketById")]
        public async Task<IActionResult> GetMarket(Guid id)
        {
            var market = await _repository.Market.GetMarketAsync(id, trackChanges: false);
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateMarket([FromBody] MarketForCreationDto market)
        {
            var marketEntity = _mapper.Map<Market>(market);
            _repository.Market.CreateMarket(marketEntity);
            await _repository.SaveAsync();
            var marketToReturn = _mapper.Map<MarketDto>(marketEntity);
            return CreatedAtRoute("MarketById", new { id = marketToReturn.Id }, marketToReturn);
        }
        [HttpGet("collection/({ids})", Name = "MarketCollection")]
        public async  Task<IActionResult> GetMarketCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var marketEntities = await _repository.Market.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != marketEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var marketsToReturn = _mapper.Map<IEnumerable<MarketDto>>(marketEntities);
            return Ok(marketsToReturn);
        }
        [HttpPost("collection")]
        public async Task<IActionResult> CreateMarketCollection([FromBody] IEnumerable<MarketForCreationDto> marketCollection)
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
            await _repository.SaveAsync();
            var marketCollectionToReturn = _mapper.Map<IEnumerable<MarketDto>>(marketEntities);
            var ids = string.Join(",", marketCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("MarketCollection", new { ids }, marketCollectionToReturn);
        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateMarketExistsAttribute))]
        public async Task<IActionResult> DeleteMarket(Guid id)
        {
            var market = HttpContext.Items["market"] as Market;
            _repository.Market.DeleteMarket(market);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateMarketExistsAttribute))]

        public async Task<IActionResult> UpdateMarket(Guid id, [FromBody] MarketForUpdateDto market)
        {
            var marketEntity = HttpContext.Items["market"] as Market;
            _mapper.Map(market, marketEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
