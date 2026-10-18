namespace LR2WebDesign.Exceptions
{
    public class UserUpdateException: Exception
    {
        public UserUpdateException(int userId, string ex)
        : base($"Ошибка при обновлении информации о пользователе с Id = {userId}: {ex}")
        {
        }
    }
}
