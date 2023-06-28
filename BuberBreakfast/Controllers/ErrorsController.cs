using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
    public class ErrorsController : ControllerBase
    {
        // Endpoint for handling general errors
        [Route("/error")]
        public IActionResult Error()
        {
            // Return a generic Problem response
            return Problem();
        }
    }
}
