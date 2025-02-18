using Cloud.Repositories;
using Domain.Entity;
using Domain.IService.Base;
using Identity.Shared.Dto.Admin.General;

namespace Domain.Interface
{
    public interface IMallProductSkuStockHistoryService : IBaseService<MallProductSkuStockHistory, MallProductSkuStockHistoryDto>
    {
        void SaveInfo(MallProductSkuStockHistory entity, ICloudUnitOfWork uw,long userId);
    }
}
