using DATA.Models;

namespace DATA.Interface
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUser(string UserName);
        Task<bool> GetUserbyPhone(string PhoneNumber);
        Task<bool> DeleteUser(int userId);
        Task<bool> UpdateUser(ApplicationUser userupdate);

        Task<bool> EditUser(ApplicationUser user);
        Task<bool> CreateUser(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();



    }
}