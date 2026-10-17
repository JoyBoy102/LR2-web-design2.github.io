using LR2WebDesign.Controllers.APIRequestBodies;

namespace LR2WebDesign.Controllers.Interfaces
{
    public interface IUserService
    {
        public Task CreateUser(UserRequestBody userBody);

        public Task UpdateUser(UserRequestBody userBody);

        public Task DeleteUser(int userId);

        public Task<UserRequestBody> GetUser(int userId);
    }
}
