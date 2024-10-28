using FurniHub.Models.UserModels;

namespace FurniHub.Services.UserServices
{
    public interface IUserService
    {
        Task<List<OutPutUser>> GetUsers();
        Task<OutPutUser> GetUserById(int id);
    }
}
