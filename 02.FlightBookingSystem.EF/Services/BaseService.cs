using _01.FlightBookingSystem.Core.Interfaces;

namespace _02.FlightBookingSystem.EF.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
