using AutoMapper;
using eAviaSales.BusinessLayer.Core.AdminActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Functions
{
    public class AdminFlow : AdminActions, IAdminAction
    {
        public AdminFlow(IMapper mapper) : base(mapper) { }

        public List<UserDto> GetAllUsers() => GetUsers();
        public List<AdminBookingDto> SearchBookings(AdminBookingsQueryDto query) => SearchBookingList(query);
    }
}
