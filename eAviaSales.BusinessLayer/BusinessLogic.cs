using AutoMapper;
using eAviaSales.BusinessLayer.Functions;
using eAviaSales.BusinessLayer.Interfaces;

namespace eAviaSales.BusinessLayer
{
    public class BusinessLogic
    {
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IMapper _mapper;

        public BusinessLogic(string jwtKey, string issuer, string audience, IMapper mapper)
        {
            _jwtKey = jwtKey;
            _issuer = issuer;
            _audience = audience;
            _mapper = mapper;
        }

        public IUserLoginAction GetUserLoginAction() => new UserLoginFlow(_jwtKey, _issuer, _audience);
        public IUserRegAction GetUserRegAction() => new UserRegFlow();

        public IUserAction GetUserAction() => new UserFlow(_mapper);
        public IProductAction GetProductAction() => new ProductFlow(_mapper);
        public IOrderAction GetOrderAction() => new OrderFlow(_mapper);

        public ITicketAction GetTicketAction() => new TicketFlow(_mapper);
        public ITripAction GetTripAction() => new TripFlow(_mapper);
        public IAdminAction GetAdminAction() => new AdminFlow(_mapper);
    }
}
