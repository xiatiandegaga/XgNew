using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Dictionary;
using Domain.IService.Dictionary;
using Domain.Service.Base;
using Identity.Shared.Dto.Admin.General;

namespace Domain.Service.Dictionary
{
    /// <summary>
    /// 图片规则明细表
    /// </summary>
    public class ImgRuleDetailService : BaseService<ImgRuleDetail, ImgRuleDetailDto>, IImgRuleDetailService
    {
        private readonly ICloudUnitOfWork _unitWork;

        public ImgRuleDetailService(IRepository<ImgRuleDetail> repository, ICloudUnitOfWork unitWork) : base(repository)
        {
            _unitWork = unitWork;
        }


    }
}
