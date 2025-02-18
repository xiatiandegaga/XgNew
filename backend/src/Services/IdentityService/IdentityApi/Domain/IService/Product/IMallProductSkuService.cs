using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Product;
using Domain.IService.Base;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Product
{
    public interface IMallProductSkuService : IBaseService<MallProductSku, MallProductSkuDto>
    {
        Task AddOrUpdateAsync(List<MallProductSkuDto> input);

        void SaveStock(List<AdminMallSkuStockChangeInfoInput> list, ICloudUnitOfWork unitWork, long userId);
    }
}
