using LR2WebDesign.Services.Entities;

namespace LR2WebDesign.Exceptions
{
    public class UserDeleteException: Exception
    {
        public UserDeleteException(int userId, string ex)
        : base($"Ошибка при удалении пользователя с Id = {userId}: {ex}")
        {
        }
    }
}
