using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
namespace LR2WebDesign.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        public Dictionary<int, User> users = new Dictionary<int, User>();
        private int incrementId = 0;

        public void Create(User user)
        {
            users.Add(incrementId++, user);
        }

        public void Delete(int userId)
        {
            users.Remove(userId);
        }

        public User GetUser(int userId)
        {
            return users[userId];
        }

        public void Update(User user)
        {
            users[user.Id] = user; 
        }
    }
}
