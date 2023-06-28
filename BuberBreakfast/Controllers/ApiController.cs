using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberBreakfast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        // This method handles the creation of a Problem response based on a list of errors.
        protected IActionResult Problem(List<Error> errors)
        {
            // Check if all errors are of type 'Validation'
            if (errors.All(e => e.Type == ErrorType.Validation))
            {
                var modelStateDictionary = new ModelStateDictionary();

                // Loop through each error and add it to the ModelStateDictionary
                foreach (var error in errors)
                {
                    modelStateDictionary.AddModelError(error.Code, error.Description);
                }

                // Return a ValidationProblem response with the ModelStateDictionary
                return ValidationProblem(modelStateDictionary);
            }

            // Check if any error is of type 'Unexpected'
            if (errors.Any(e => e.Type == ErrorType.Unexpected))
            {
                // Return a generic Problem response
                return Problem();
            }

            // Get the first error from the list
            var firstError = errors[0];

            // Determine the appropriate HTTP status code based on the error type
            var statusCode = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            // Return a Problem response with the determined status code and error description as the title
            return Problem(statusCode: statusCode, title: firstError.Description);
        }
    }
}
