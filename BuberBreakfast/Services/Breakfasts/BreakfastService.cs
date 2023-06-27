using BuberBreakfast.Models;

namespace BuberBreakfast.Services.Breakfasts
{
    public class BreakfastService : IBreakfastService
    {
        private readonly Dictionary<Guid, Breakfast> _breakfasts = new();
        public void CreateBreakfast(Breakfast breakfast) 
        {
            _breakfasts.Add(breakfast.Id, breakfast);
        }
    }
}
