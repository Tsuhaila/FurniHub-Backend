namespace FurniHub.Models.UserModels.DTOs
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public bool IsBlocked { get; set; }
    }
}
