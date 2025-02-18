using Cloud.Models;
using Domain.Entity.Product;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Product
{
    public interface IMallProductService : IBaseListCacheService<MallProduct, MallProductDto>
    {

        Task<MallProductDto> FindSingleByIdForH5Async(IdQueryCommonInput input);

        List<MallProduct> GetAllListByTypeId(long typeId);

        Task<List<MallProduct>> GetAllListAsync();
        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        Task TakeOnAsync(long id);
        
        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        Task TakeOffAsync(long id);
        Task BatchTakeOnAsync(List<long> listId);

        Task BatchTakeOffAsync(List<long> listId);

        /// <summary>
        /// 通过类型编号获取所有上架商品并分类分组
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        Task<dynamic> GetProductsGroupbyCategoryByTypeCode(string typeCode);

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public Task StockInv(AdminMallSkuStockChangeInput input);
        

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        public Task StockRel(AdminMallSkuStockChangeInput input);
    }
}
