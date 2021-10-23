using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Exchange.Infrastructure.Attributes
{
    public class ModelValidatorAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ModelValidatorAttribute> _logger;

        public ModelValidatorAttribute(ILogger<ModelValidatorAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var firstMessage = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                                                              .SelectMany(v => v.Errors)
                                                              .Select(v => new
                                                              {
                                                                  Message = (!string.IsNullOrEmpty(v.ErrorMessage) || v.Exception == null) ? v.ErrorMessage : v.Exception.Message
                                                              })
                                                              .FirstOrDefault();

                _logger.LogWarning($"Model is not valid. {firstMessage.Message}");

                context.Result = new BadRequestObjectResult(new
                {
                    message = firstMessage.Message
                });
            }
        }
    }
}
