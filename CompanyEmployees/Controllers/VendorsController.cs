using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyEmployees.Controllers
{
    [Route("api/markets/{marketId}/vendors")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<VendorDto> _dataShaper;
        public VendorsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<VendorDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetVendorsForMarket(Guid marketId, [FromQuery] VendorParameters vendorParameters)
        {
            var market = await _repository.Market.GetMarketAsync(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorsFromDb = await _repository.Vendor.GetVendorsAsync(marketId, vendorParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(vendorsFromDb.MetaData));
            var vendorsDto = _mapper.Map<IEnumerable<VendorDto>>(vendorsFromDb);
            return Ok(_dataShaper.ShapeData(vendorsDto, vendorParameters.Fields));
        }
        [HttpGet("{id}", Name = "GetVendorForMarket")]
        public async Task<IActionResult> GetVendorForMarket(Guid marketId, Guid id)
        {
            var market = await _repository.Market.GetMarketAsync(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorDb = await _repository.Vendor.GetVendorAsync(marketId, id, trackChanges: false);
            if (vendorDb == null)
            {
                _logger.LogInfo($"Vendor with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var vendor = _mapper.Map<VendorDto>(vendorDb);
            return Ok(vendor);
        }
        [HttpPost]
        public async Task<IActionResult> CreateVendorForMarket(Guid marketId, [FromBody] VendorForCreationDto vendor)
        {
            if (vendor == null)
            {
                _logger.LogError("VendorForCreationDto object sent from client is null.");
                return BadRequest("VendorForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the VendorForCreationDto object");
                return UnprocessableEntity(ModelState);
            }
            var market = await _repository.Market.GetMarketAsync(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorEntity = _mapper.Map<Vendor>(vendor);
            _repository.Vendor.CreateVendorForMarket(marketId, vendorEntity);
            await _repository.SaveAsync();
            var vendorToReturn = _mapper.Map<VendorDto>(vendorEntity);
            return CreatedAtRoute("GetVendorForMarket", new
            {
                marketId,
                id = vendorToReturn.Id
            }, vendorToReturn);
        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateVendorForMarketExistsAttribute))]
        public async Task<IActionResult> DeleteVendorForMarket(Guid marketId, Guid id)
        {
            var vendorForMarket = HttpContext.Items["vendor"] as Vendor;
            _repository.Vendor.DeleteVendor(vendorForMarket);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateVendorForMarketExistsAttribute))]
        public async Task<IActionResult> UpdateVendorForMarket(Guid marketId, Guid id, [FromBody] VendorForUpdateDto vendor)
        {
            var vendorEntity = HttpContext.Items["vendor"] as Vendor;
            _mapper.Map(vendor, vendorEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateVendorForMarketExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateVendorForMarket(Guid marketId, Guid id, [FromBody] JsonPatchDocument<VendorForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var vendorEntity = HttpContext.Items["vendor"] as Vendor;
            var vendorToPatch = _mapper.Map<VendorForUpdateDto>(vendorEntity);
            patchDoc.ApplyTo(vendorToPatch, ModelState);
            TryValidateModel(vendorToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(vendorToPatch, vendorEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
