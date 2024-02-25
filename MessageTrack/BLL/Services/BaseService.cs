using MessageTrack.BLL.Interfaces;
using MessageTrack.DAL.Interfaces.UnitOfWork;

namespace MessageTrack.BLL.Services
{
    public class BaseService : IBaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public void Rollback()
        {
            _unitOfWork.Rollback();
        }
    }
}
