using Domain.Entity.Identity;
using Domain.Entity.Order;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Mapster;

namespace Application.AutoMapper
{
    public class InitConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<Role, RoleDto>()
                      .Map(dest => dest.Name, src => src.Name);
            config.ForType<MallOrderReturn, MallOrderReturnDto>()
                     .Map(dest => dest.DetailStatus, src => src.MallOrderDetail.Status);
        }
    }
}
