using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Exceptions;
using LR2WebDesign.Services;
using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net.WebSockets;
using System.Timers;

namespace Tests.ServiceTests
{
    public class UserServiceTests
    {

        /*Создание, удаление, получение, если пользователя нету, есть, есть соответственно */
        [Fact]
        public async Task CreateUser_UserDoesNotExist_CallsRepositoryCreateOnlyOnceWithCorrectParameters()
        {
            var mockUserRepository = new Mock<IUserRepository>();

            mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<User>()))
                              .ReturnsAsync((false, "", null));

            mockUserRepository
                .Setup(repo => repo.Create(It.IsAny<User>()));

            var mockConfig = CreateMockConfiguration();

            var userService = new UserService(mockUserRepository.Object);

            var request = new UserCreateRequestDTO
            {
                Login = "new_user",
                Name = "",
                Password = "password123",
                Email = "ivan@example.com"
            };


            await userService.CreateUser(request, mockConfig);
            mockUserRepository.Verify(
                repo => repo.Create(It.Is<User>(u => u.Login == request.Login &&
                                                     u.Password == request.Password &&
                                                     u.Name == request.Name &&
                                                     u.Email == request.Email)),
                Times.Once
            );
        }

        /* -------------------------------------------------------------------------------------------------- */

        /*Создание, удаление, получение, если пользователь есть, нету, нету соответственно */

        [Fact]
        public async Task CreateUser_UserExists_CallsRepositoryCreateOnlyOnceWithCorrectParametersAndReturnCorrectException()
        {
            var mockUserRepository = new Mock<IUserRepository>();

            mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<User>()))
                              .ReturnsAsync((true, "Login", new User
                              {
                                  Login = "new_user",
                                  Name = "",
                                  Password = "password123",
                                  Email = "ivan@example.com"
                              }));

            mockUserRepository
                .Setup(repo => repo.Create(It.IsAny<User>()));

            var mockConfig = CreateMockConfiguration();

            var userService = new UserService(mockUserRepository.Object);

            var request = new UserCreateRequestDTO
            {
                Login = "new_user",
                Name = "",
                Password = "password123",
                Email = "ivan@example.com"
            };


            var exception = await Assert.ThrowsAsync<UserAlreadyExistsException>(() => userService.CreateUser(request, mockConfig));
            Assert.Equal($"Пользователь с таким Login уже существует.", exception.Message);

            mockUserRepository.Verify(
                repo => repo.Create(It.Is<User>(u => u.Login == request.Login &&
                                                     u.Password == request.Password &&
                                                     u.Name == request.Name &&
                                                     u.Email == request.Email)),
                Times.Never
            );
            
           

        }

        // =====================================================================
        // ТЕСТ 1: Пользователь существует, логин и пароль верные (Успех)
        // =====================================================================
        [Fact]
        public async Task LoginUser_UserExistsAndCredentialsValid_ReturnsTokenString()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockUserRepository.Object);
            var config = CreateMockConfiguration();

            var request = new UserLoginRequestDTO
            {
                LoginOrEmail = "test_user",
                Password = "password123"
            };

            var dbUser = new User
            {
                Login = "test_user",
                Password = "password123",
                Email = "test@example.com"
            };

            mockUserRepository
                .Setup(repo => repo.GetUser(It.IsAny<User>()))
                .ReturnsAsync((true, "OK", dbUser));

            var result = await userService.LoginUser(request, config);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<string>(result);
        }

        // =====================================================================
        // ТЕСТ 2: Пользователя нет в базе данных (Исключение GetUserException)
        // =====================================================================
        [Fact]
        public async Task LoginUser_UserDoesNotExist_ThrowsGetUserException()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockUserRepository.Object);
            var config = CreateMockConfiguration();

            var request = new UserLoginRequestDTO
            {
                LoginOrEmail = "unknown_user",
                Password = "password123"
            };

            mockUserRepository
                .Setup(repo => repo.GetUser(It.IsAny<User>()))
                .ReturnsAsync((false, "", null));

            var exception = await Assert.ThrowsAsync<GetUserException>(() =>
                userService.LoginUser(request, config));

            Assert.Equal("Пользователь не найден.", exception.Message);
        }

        // =====================================================================
        // ТЕСТ 3: Пользователь есть, но пароль неверный (Исключение InvalidCredentialsException)
        // =====================================================================
        [Fact]
        public async Task LoginUser_UserExistsButWrongPassword_ThrowsInvalidCredentialsException()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockUserRepository.Object);
            var config = CreateMockConfiguration();

            var request = new UserLoginRequestDTO
            {
                LoginOrEmail = "test_user",
                Password = "wrong_password"
            };

            var dbUser = new User
            {
                Login = "test_user",
                Password = "password123",
                Email = "test@example.com"
            };

            mockUserRepository
                .Setup(repo => repo.GetUser(It.IsAny<User>()))
                .ReturnsAsync((true, "OK", dbUser));

            await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
                userService.LoginUser(request, config));
        }

        private IConfiguration CreateMockConfiguration()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["Jwt:Key"]).Returns("super_secret_key_at_least_32_characters_long!!");
            mockConfig.Setup(x => x["Jwt:Issuer"]).Returns("https://my-app.com");
            mockConfig.Setup(x => x["Jwt:Audience"]).Returns("https://my-api.com");
            return mockConfig.Object;
        }
    }
}
