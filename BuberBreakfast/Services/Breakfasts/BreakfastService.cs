using BuberBreakfast.Models;
using BuberBreakfast.ServicesErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts
{
    public class BreakfastService : IBreakfastService
    {
        private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();

        public void CreateBreakfast(Breakfast breakfast) 
        {
            _breakfasts.Add(breakfast.Id, breakfast);
        }

        public void DeleteBreakfast(Guid id)
        {
            _breakfasts.Remove(id);
        }

        public ErrorOr<Breakfast> GetBreakfast(Guid id)
        {
            if( _breakfasts.TryGetValue(id, out Breakfast breakfast)) return breakfast;

            return Errors.Breakfast.NotFound;
        }

        public void UpsertBreakfast(Breakfast breakfast)
        {
            _breakfasts[breakfast.Id] = breakfast;
        }
    }
}
