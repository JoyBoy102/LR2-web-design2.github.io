namespace LR2WebDesign.Services.Entities
{
    public class User
    {
        public int Id { get; set; } = -1;
        public string? Login {  get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }
    }
}
