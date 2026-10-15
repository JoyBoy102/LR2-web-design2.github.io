using LR2WebDesign.Controllers.APIRequestBodies;

namespace LR2WebDesign.Controllers.Interfaces
{
    public interface IUserService
    {
        public void CreateUser(UserRequestBody userBody);

        public void UpdateUser(UserRequestBody userBody);

        public void DeleteUser(int userId);

        public UserRequestBody GetUser(int userId);
    }
}
