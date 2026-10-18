namespace LR2WebDesign.Exceptions
{
    public class GetUserException: Exception
    {
        public GetUserException(int userId, string ex)
        : base($"Ошибка при получении пользователя с Id = {userId}: {ex}")
        {
        }
    }
}
