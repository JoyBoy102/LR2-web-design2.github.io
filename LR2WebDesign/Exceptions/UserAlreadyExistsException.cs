namespace LR2WebDesign.Exceptions
{
    public class UserAlreadyExistsException:  Exception
    {
        public UserAlreadyExistsException(string type)
        : base($"Пользователь с таким {type} уже существует.")
        {
        }
    }
}
