using CarRentalBll.Configurations;
using CarRentalBll.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace CarRentalWeb.Validation
{
    public class AdminMinimumAgeFilter: ActionFilterAttribute
    {
        private readonly UserRequirements _userRequirements;

        public AdminMinimumAgeFilter(IOptions<UserRequirements> userRequirementsOptions)
        {
            _userRequirements = userRequirementsOptions.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var value = context.ModelState["DateOfBirth"]?.RawValue;

            if (value is not DateTime valueAsDateTime)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            var minimumAge = _userRequirements.AdminMinimumAge;

            Console.WriteLine(minimumAge);
            Console.WriteLine(valueAsDateTime);

            if (UserService.CheckIfHasAge(valueAsDateTime, minimumAge))
            {
                return;
            }

            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}