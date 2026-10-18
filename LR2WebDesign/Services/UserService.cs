using LR2WebDesign.Controllers.Interfaces;
using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Exceptions;

namespace LR2WebDesign.Services
{
    public class UserService: IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUser(UserRequestBody userRequestBody)
        {
            User user = new User
            {
                Id = userRequestBody.Id,
                Login = userRequestBody.Login,
                Name = userRequestBody.Name,
                Password = userRequestBody.Password,
                Email = userRequestBody.Email,
            };

            var userExistsObj = await _userRepository.UserExists(user);

            if (userExistsObj.Item1)
            {
                throw new UserAlreadyExistsException(userExistsObj.Item2);
            }
            else
            { 
                await _userRepository.Create(user);
            }
            
        }

        public async Task UpdateUser(UserRequestBody userRequestBody)
        {
            await DeleteUser(userRequestBody.Id);
            await CreateUser(userRequestBody);
        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                var user = await _userRepository.GetUser(userId);
                await _userRepository.Delete(user.Id);
            }
            
            catch (Exception ex)
            {
                throw new UserDeleteException(userId, ex.Message);
            }
        }

        public async Task<UserRequestBody> GetUser(int userId)
        {
            try
            {
                User user = await _userRepository.GetUser(userId);
                return new UserRequestBody
                {
                    Id = userId,
                    Login = user.Login,
                    Name = user.Name,
                    Password = user.Password,
                    Email = user.Email,
                };
            }
            catch (Exception ex)
            {
               throw new GetUserException(userId, ex.Message);
            }
        }
    }
}
