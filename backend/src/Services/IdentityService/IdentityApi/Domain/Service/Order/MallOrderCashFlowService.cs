using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity;
using Domain.IService.Identity;
using Domain.Service.Base;
using Identity.Shared.Dto.Admin.General;

namespace Domain.Service.Order
{
    /// <summary>
    /// 
    /// </summary>
    public class MallOrderCashFlowService : BaseCacheService<MallOrderCashFlow, MallOrderCashFlowDto>, IMallOrderCashFlowService
    {
        private readonly ICloudUnitOfWork _unitWork;

        public MallOrderCashFlowService(ICacheRepository<MallOrderCashFlow> repository, ICloudUnitOfWork unitWork) : base(repository)
        {
            _unitWork = unitWork;
        }


    }
}
