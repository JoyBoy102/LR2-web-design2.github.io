using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
namespace LR2WebDesign.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        public Dictionary<int, User> users = new Dictionary<int, User>();
        private int incrementId = 0;

        public async Task Create(User user)
        {
            users.Add(incrementId++, user);
        }

        public async Task Delete(int userId)
        {
            users.Remove(userId);
        }

        public async Task<User> GetUser(int userId)
        {
            return users[userId];
        }

        public async Task Update(User user)
        {
            users[user.Id] = user; 
        }

        public async Task<(bool, string)> UserExists(User newUser)
        {
            foreach (var user in users)
            {
                if (user.Value.Email == newUser.Email)
                    return (true, "Email");
                else if (user.Value.Login == newUser.Login)
                    return (true, "Login");
            }
            return (false, "");
        }
    }
}
