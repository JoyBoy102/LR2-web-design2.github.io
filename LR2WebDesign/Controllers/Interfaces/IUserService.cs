using LR2WebDesign.Controllers.APIRequestBodies;

namespace LR2WebDesign.Controllers.Interfaces
{
    public interface IUserService
    {
        public Task<string> CreateUser(UserCreateRequestDTO userCreateRequest, IConfiguration config);

        public Task<string> LoginUser(UserLoginRequestDTO userLoginRequest, IConfiguration config);
    }
}
