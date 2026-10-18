using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Exceptions;
using LR2WebDesign.Services;
using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
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
            var userService = new UserService(mockUserRepository.Object);

            var request = new UserRequestBody
            {
                Login = "new_user",
                Name = "",
                Password = "password123",
                Email = "ivan@example.com"
            };
            mockUserRepository
                .Setup(repo => repo.UserExists(It.IsAny<User>()))
                .ReturnsAsync((false, string.Empty));
            await userService.CreateUser(request);
            mockUserRepository.Verify(
                repo => repo.Create(It.Is<User>(u => u.Login == request.Login &&
                                                     u.Password == request.Password &&
                                                     u.Name == request.Name &&
                                                     u.Email == request.Email)),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteUser_UserExists_CallsRepositoryDeleteOnlyOnceWithCorrectParameters()
        {
            var deleteId = 4;
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<int>()))
                              .ReturnsAsync(new User { Id = deleteId, Login = "test", Name = "Ivan", Password = "123", Email = "a@a.com" });
            var userService = new UserService(mockUserRepository.Object);
            await userService.DeleteUser(deleteId);
            mockUserRepository.Verify(
                repo => repo.Delete(It.Is<int>(u => u == deleteId)),
                Times.Once
            );
        }

        [Fact]
        public async Task GetUser_UserExists_CallsRepositoryGetUserOnlyOnceWithCorrectParameters()
        {
            var getId = 4;
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
               .Setup(repo => repo.GetUser(getId))
               .ReturnsAsync(new User { Login = "test", Name = "Ivan", Password = "123", Email = "a@a.com" });
            var userService = new UserService(mockUserRepository.Object);
            await userService.GetUser(getId);
            mockUserRepository.Verify(
                repo => repo.GetUser(It.Is<int>(u => u == getId)),
                Times.Once
            );
        }

        /* -------------------------------------------------------------------------------------------------- */

        /*Создание, удаление, получение, если пользователь есть, нету, нету соответственно */

        [Fact]
        public async Task CreateUser_UserExists_CallsRepositoryCreateOnlyOnceWithCorrectParametersAndReturnCorrectException()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockUserRepository.Object);

            var request = new UserRequestBody
            {
                Login = "new_user",
                Name = "",
                Password = "password123",
                Email = "ivan@example.com"
            };
            mockUserRepository
                .Setup(repo => repo.UserExists(It.IsAny<User>()))
                .ReturnsAsync((true, "Login"));


            var exception = await Assert.ThrowsAsync<UserAlreadyExistsException>(() => userService.CreateUser(request));
            Assert.Equal($"Пользователь с таким Login уже существует.", exception.Message);

            mockUserRepository.Verify(
                repo => repo.Create(It.Is<User>(u => u.Login == request.Login &&
                                                     u.Password == request.Password &&
                                                     u.Name == request.Name &&
                                                     u.Email == request.Email)),
                Times.Never
            );
            
           

        }

        [Fact]
        public async Task DeleteUser_UserDoesNotExists_CallsRepositoryDeleteOnlyOnceWithCorrectParametersAndReturnCorrectException()
        {
            var deleteId = 4;
            var mockUserRepository = new Mock<IUserRepository>();

            mockUserRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
                              .ThrowsAsync(new ArgumentException($"key {deleteId} не найден"));

            mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<int>()))
                              .ThrowsAsync(new ArgumentException($"key {deleteId} не найден"));

            var userService = new UserService(mockUserRepository.Object);
            var exception = await Assert.ThrowsAsync<UserDeleteException>(() => userService.DeleteUser(deleteId));
            Assert.Contains($"Ошибка при удалении пользователя с Id = {deleteId}", exception.Message);
            Assert.Contains($"key {deleteId} не найден", exception.Message);
            mockUserRepository.Verify(
                repo => repo.Delete(It.Is<int>(u => u == deleteId)),
                Times.Never
            );
        }

        [Fact]
        public async Task GetUser_UserDoesNotExists_CallsRepositoryGetUserOnlyOnceWithCorrectParametersAndReturnCorrectException()
        {
            var getId = 4;
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<int>()))
                              .ThrowsAsync(new ArgumentException($"key {getId} не найден"));

            var userService = new UserService(mockUserRepository.Object);
            
            var exception = await Assert.ThrowsAsync<GetUserException>(() => userService.GetUser(getId));

            Assert.Contains($"Ошибка при получении пользователя с Id = {getId}", exception.Message);
            Assert.Contains($"key {getId} не найден", exception.Message);

            mockUserRepository.Verify(
                repo => repo.GetUser(It.Is<int>(u => u == getId)),
                Times.Once
            );
        }
    }
}
