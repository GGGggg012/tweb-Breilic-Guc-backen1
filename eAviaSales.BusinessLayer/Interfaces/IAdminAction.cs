using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface IAdminAction
    {
        List<UserDto> GetAllUsers();
        List<AdminBookingDto> SearchBookings(AdminBookingsQueryDto query);
    }
}
