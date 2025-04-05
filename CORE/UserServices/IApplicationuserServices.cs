using DATA.DTO;
using DATA.Models;

namespace CORE.UserServices
{
    public interface IApplicationuserServices
    {
        Task<ResponseDto> DeleteUser(int userId);
        Task<ResponseDto> UpdateUser(ApplicationUser userUpdate);
        Task<ResponseDto> EditUser(ApplicationUser user);
        Task<ResponseDto> CreateUser(UserRequestDto user);

        Task<ResponseDto> GetUserByUsername(string userName);

        Task<ResponseDto> GetAllUsers();
        Task<LoginResponseDto> UserLogin(LoginRequestDto user);

    }
}
