namespace LR2WebDesign.Exceptions
{
    public class InvalidCredentialsException: Exception
    {
        public InvalidCredentialsException()
        : base($"Неверный пароль.")
        {
        }
    }
}
