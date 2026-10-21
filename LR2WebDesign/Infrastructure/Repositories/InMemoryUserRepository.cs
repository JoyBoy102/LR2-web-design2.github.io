using LR2WebDesign.Services.Entities;
using LR2WebDesign.Services.Interfaces;
namespace LR2WebDesign.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        public Dictionary<int, User> users = StaticMemoryDB.users;

        public async Task Create(User user)
        {
            user.Id = StaticMemoryDB.incrementId;

            users.Add(StaticMemoryDB.incrementId, user);

            StaticMemoryDB.incrementId++;
        }

        public async Task Delete(int userId)
        {
            if (!users.ContainsKey(userId))
            {
                throw new KeyNotFoundException($"Пользователь с ID {userId} не найден");
            }

            users.Remove(userId);
        }

        public async Task<User> GetUser(int userId)
        {
            return users.GetValueOrDefault(userId);
        }

        public async Task Update(User user)
        {
            throw new NotImplementedException(); 
        }

        public async Task<(bool, string, User?)> GetUser(User newUser)
        {
            foreach (var user in users)
            {
                if (user.Value.Email!=null && user.Value.Email == newUser.Email)
                    return (true, "Email", user.Value);
                else if (user.Value.Login!=null && user.Value.Login == newUser.Login)
                    return (true, "Login", user.Value);
            }
            return (false, "", null);
        }
    }
}
