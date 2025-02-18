using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Order;
using Domain.IService.Order;
using Domain.Service.Base;
using Identity.Shared.Dto.Admin.General;

namespace Domain.Service.Order
{
    /// <summary>
    /// 
    /// </summary>
    public class MallOrderDetailService : BaseService<MallOrderDetail, MallOrderDetailDto>, IMallOrderDetailService
    {
        private readonly ICloudUnitOfWork _unitWork;

        public MallOrderDetailService(IRepository<MallOrderDetail> repository, ICloudUnitOfWork unitWork) : base(repository)
        {
            _unitWork = unitWork;
        }


    }
}
