using Cloud.Models;
using Cloud.WebApiClient;
using Identity.Shared.Dto;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Identity.Shared.Contracts
{
    public interface IAuthService : ICloudRemoteService
    {
        [HttpGet("api/Login/GetAuthenticatedUser")]
        Task<AjaxResponseGen<UserDto>> GetAuthenticatedUser();
    }
}
