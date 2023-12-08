using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/markets/{marketId}/vendors")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public VendorsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetVendorsForMarket(Guid marketId)
        {
            var market = _repository.Market.GetMarket(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorsFromDb = _repository.Vendor.GetVendors(marketId, trackChanges: false);
            var vendorsDto = _mapper.Map<IEnumerable<VendorDto>>(vendorsFromDb);
            return Ok(vendorsDto);
        }
        [HttpGet("{id}", Name = "GetVendorForMarket")]
        public IActionResult GetVendorForMarket(Guid marketId, Guid id)
        {
            var market = _repository.Market.GetMarket(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorDb = _repository.Vendor.GetVendor(marketId, id, trackChanges: false);
            if (vendorDb == null)
            {
                _logger.LogInfo($"Vendor with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var vendor = _mapper.Map<VendorDto>(vendorDb);
            return Ok(vendor);
        }
        [HttpPost]
        public IActionResult CreateVendorForMarket(Guid marketId, [FromBody] VendorForCreationDto vendor)
        {
            if (vendor == null)
            {
                _logger.LogError("VendorForCreationDto object sent from client is null.");
                return BadRequest("VendorForCreationDto object is null");
            }
            var market = _repository.Market.GetMarket(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorEntity = _mapper.Map<Vendor>(vendor);
            _repository.Vendor.CreateVendorForMarket(marketId, vendorEntity);
            _repository.Save();
            var vendorToReturn = _mapper.Map<VendorDto>(vendorEntity);
            return CreatedAtRoute("GetVendorForMarket", new
            {
                marketId,
                id = vendorToReturn.Id
            }, vendorToReturn);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteVendorForMarket(Guid marketId, Guid id)
        {
            var market = _repository.Market.GetMarket(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorForMarket = _repository.Vendor.GetVendor(marketId, id, trackChanges: false);
            if (vendorForMarket == null)
            {
                _logger.LogInfo($"Vendor with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Vendor.DeleteVendor(vendorForMarket);
            _repository.Save();
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateVendorForMarket(Guid marketId, Guid id, [FromBody] VendorForUpdateDto vendor)
        {
            if (vendor == null)
            {
                _logger.LogError("VendorForUpdateDto object sent from client is null.");
                return BadRequest("VendorForUpdateDto object is null");
            }
            var market = _repository.Market.GetMarket(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorEntity = _repository.Vendor.GetVendor(marketId, id, trackChanges: true);
            if (vendorEntity == null)
            {
                _logger.LogInfo($"Vendor with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(vendor, vendorEntity);
            _repository.Save();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateVendorForMarket(Guid marketId, Guid id, [FromBody] JsonPatchDocument<VendorForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var market = _repository.Market.GetMarket(marketId, trackChanges: false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return NotFound();
            }
            var vendorEntity = _repository.Vendor.GetVendor(marketId, id, trackChanges: true);
            if (vendorEntity == null)
            {
                _logger.LogInfo($"Vendor with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var employeeToPatch = _mapper.Map<VendorForUpdateDto>(vendorEntity);
            patchDoc.ApplyTo(employeeToPatch);
            _mapper.Map(employeeToPatch, vendorEntity);
            _repository.Save();
            return NoContent();
        }
    }
}
