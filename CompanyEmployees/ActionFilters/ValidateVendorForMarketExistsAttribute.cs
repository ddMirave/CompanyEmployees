using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.ActionFilters
{
    public class ValidateVendorForMarketExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateVendorForMarketExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var marketId = (Guid)context.ActionArguments["marketId"];
            var market = await _repository.Market.GetMarketAsync(marketId, false);
            if (market == null)
            {
                _logger.LogInfo($"Market with id: {marketId} doesn't exist in the database.");
                return;
                context.Result = new NotFoundResult();
            }
            var id = (Guid)context.ActionArguments["id"];
            var vendor = await _repository.Vendor.GetVendorAsync(marketId, id, trackChanges);
            if (vendor == null)
            {
                _logger.LogInfo($"Vendor with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("vendor", vendor);
                await next();
            }
        }
    }
}
