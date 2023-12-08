using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies")]
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
        [HttpGet("{id}")]
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
    }
}
