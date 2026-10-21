using LR2WebDesign.Services.Entities;

namespace LR2WebDesign
{
    public class StaticMemoryDB
    {
        public static Dictionary<int, User> users = new Dictionary<int, User>
        {
            { 0, new User { Email = "user@example.com", Id = 0, Login = "user121", Name = "Ajnur", Password = "Password123@" } }
        };
        public static int incrementId = 1;
    }
}
