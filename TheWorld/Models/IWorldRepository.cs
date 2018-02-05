using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        void AddTrip(Trip trip);
        Task<bool> SaveChangesAsync();

        Trip GetTripByName(string tripName);
        IEnumerable<Trip> GetTripsByUserName(string name);

        void AddStop(string tripName, Stop newStop, string userName);
        Trip GetUserTripByName(string tripName, string name);
    }
}
