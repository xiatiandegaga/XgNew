using Domain.Entity.Dictionary;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Dictionary
{
    public interface IImgRuleService : IBaseListCacheService<ImgRule, ImgRuleDto>
    {
        Task<ImgRuleInfoDto> FindSingleDetailsByIdAsync([FromBody] IdQueryCommonInput input);

        Task AddOrUpdateAsync([FromBody] ImgRuleInfoDto input);

        Task<IEnumerable<ImgRuleInfoDto>> GetAllDetailsList();

        Task<List<ImgRuleInfoDto>> GetListByCodesAsync([FromBody] CodesQueryCommonInput input);
    }
}
