namespace LR2WebDesign.Controllers.APIRequestBodies
{
    public class UserRequestBody
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
