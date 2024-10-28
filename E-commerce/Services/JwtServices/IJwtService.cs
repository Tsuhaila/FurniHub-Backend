namespace FurniHub.Services.JwtServices
{
    public interface IJwtService
    {
        int GetUserIdFromToken(string token);
    }
}
