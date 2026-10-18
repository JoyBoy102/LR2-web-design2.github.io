using LR2WebDesign.Services.Entities;

namespace LR2WebDesign
{
    public class StaticMemoryDB
    {
        public static Dictionary<int, User> users = new Dictionary<int, User>();
        public static int incrementId = 0;
    }
}
