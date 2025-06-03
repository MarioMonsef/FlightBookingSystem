using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _03.FlightBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper _mapper)
        {
            this._mapper = _mapper;
        }
    }
}
