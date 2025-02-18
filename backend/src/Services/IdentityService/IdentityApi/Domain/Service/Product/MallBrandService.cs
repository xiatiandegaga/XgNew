using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Product;
using Domain.IService.Product;
using Domain.Service.Base;
using Identity.Shared.Dto.Admin.General;

namespace Domain.Service.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class MallBrandService : BaseService<MallBrand,MallBrandDto>, IMallBrandService
    {
        private readonly ICloudUnitOfWork _unitWork;

        public MallBrandService(IRepository<MallBrand> repository,ICloudUnitOfWork unitWork) : base(repository)
        {
            _unitWork = unitWork;
        }


    }
}
