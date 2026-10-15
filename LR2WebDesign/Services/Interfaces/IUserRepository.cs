using LR2WebDesign.Services.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace LR2WebDesign.Services.Interfaces
{
    public interface IUserRepository
    {
        public void Create(User user);
        public void Update(User user);

        public void Delete(int userId);

        public User GetUser(int userId);

    }
}
