using LR2WebDesign.Controllers.Interfaces;
using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
using LR2WebDesign.Controllers.APIRequestBodies;

namespace LR2WebDesign.Services
{
    public class UserService: IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(UserRequestBody userRequestBody)
        {
            User user = new User
            {
                Login = userRequestBody.Login,
                Name = userRequestBody.Name,
                Password = userRequestBody.Password,
                Email = userRequestBody.Email,
            };
            _userRepository.Create(user);
        }

        public void UpdateUser(UserRequestBody userRequestBody)
        {
            DeleteUser(userRequestBody.Id);
            CreateUser(userRequestBody);
        }

        public void DeleteUser(int userId)
        {
            _userRepository.Delete(userId);
        }

        public UserRequestBody GetUser(int userId)
        {
            User user = _userRepository.GetUser(userId);
            return new UserRequestBody
            {
                Id = userId,
                Login = user.Login,
                Name = user.Name,
                Password = user.Password,
                Email = user.Email,
            };
        }
    }
}
