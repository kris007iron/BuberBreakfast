using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using BuberBreakfast.ServicesErrors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
    public class BreakfastsController : ApiController
    {
        private readonly IBreakfastService _breakfastService;

        public BreakfastsController(IBreakfastService breakfastService)
        {
            _breakfastService = breakfastService;
        }

        // Endpoint for creating a new breakfast
        [HttpPost]
        public IActionResult CreateBreakfast(CreateBreakfastRequest request)
        {
            // Create a Breakfast object from the request parameters
            ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.Create(request.Name, request.Description, request.StartDateTime, request.EndDateTime, request.Savory, request.Sweet);

            // If there is an error in creating the Breakfast object, return a Problem response
            if (requestToBreakfastResult.IsError)
            {
                return Problem(requestToBreakfastResult.Errors);
            }

            var breakfast = requestToBreakfastResult.Value;

            // Create the breakfast using the BreakfastService
            ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

            // Match the result to return the appropriate response
            return createBreakfastResult.Match(
                created => CreatedAtGetBreakfast(breakfast),
                errors => Problem(errors));
        }

        // Endpoint for getting a specific breakfast by ID
        [HttpGet("{id:guid}")]
        public IActionResult GetBreakfast(Guid id)
        {
            // Get the breakfast using the BreakfastService
            ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

            // Match the result to return the appropriate response
            return getBreakfastResult.Match(
                breakfast => Ok(MapBreakfastResponse(breakfast)),
                errors => Problem(errors)
                );
        }

        private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
        {
            // Map the Breakfast object to a BreakfastResponse object
            return new BreakfastResponse(breakfast.Id, breakfast.Name, breakfast.Description, breakfast.StartDateTime, breakfast.EndDateTime, breakfast.LastModifiedDateTime, breakfast.Savory, breakfast.Sweet);
        }

        // Endpoint for updating or inserting a breakfast
        [HttpPut("{id:guid}")]
        public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
        {
            // Create or update a Breakfast object from the request parameters
            ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.Create(request.Name, request.Description, request.StartDateTime, request.EndDateTime, request.Savory, request.Sweet, id);

            // If there is an error in creating/updating the Breakfast object, return a Problem response
            if (requestToBreakfastResult.IsError)
            {
                return Problem(requestToBreakfastResult.Errors);
            }

            var breakfast = requestToBreakfastResult.Value;

            // Upsert the breakfast using the BreakfastService
            ErrorOr<UpsertedBreakfast> upsertBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);

            // Match the result to return the appropriate response
            return upsertBreakfastResult.Match(
                upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
                errors => Problem(errors));
        }

        // Endpoint for deleting a breakfast by ID
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBreakfast(Guid id)
        {
            // Delete the breakfast using the BreakfastService
            ErrorOr<Deleted> deleteBreakfastResult = _breakfastService.DeleteBreakfast(id);

            // Match the result to return the appropriate response
            return deleteBreakfastResult.Match(
                deleted => NoContent(),
                errors => Problem(errors));
        }

        private CreatedAtActionResult CreatedAtGetBreakfast(Breakfast breakfast)
        {
            // Return a CreatedAtAction response with the created breakfast
            return CreatedAtAction(
                actionName: nameof(GetBreakfast),
                routeValues: new { id = breakfast.Id },
                value: MapBreakfastResponse(breakfast));
        }
    }
}
