using LR2WebDesign.Services.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace LR2WebDesign.Services.Interfaces
{
    public interface IUserRepository
    {
        public Task Create(User user);
        public Task Update(User user);

        public Task Delete(int userId);

        public Task<User> GetUser(int userId);

        public Task<(bool, string, User?)> GetUser(User newUser);

    }
}
