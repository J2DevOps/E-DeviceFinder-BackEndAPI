using DATA.DTO;
using DATA.Interface;
using DATA.Models;
using Microsoft.AspNetCore.Identity;

namespace CORE.UserServices
{
    public class ApplicationuserServices : IApplicationuserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;


        /// <summary>
        /// Service class for user-related operations.
        /// Uses UserRepository for database interactions and returns standardized ResponseDto.
        /// </summary>
        private readonly IUserRepository _userRepository;



        /// <summary>
        /// Constructor to inject the user repository.
        /// </summary>
        /// <param name="userRepository">Instance of IUserRepository.</param>
        public ApplicationuserServices(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="userName">Username of the user.</param>
        /// <returns>ResponseDto with user details or error message.</returns>
        public async Task<ResponseDto> GetUserByUsername(string userName)
        {
            var user = await _userRepository.GetUser(userName);
            return user != null
                ? new ResponseDto { StatusCode = 200, Message = "User found", Result = user }
                : new ResponseDto { StatusCode = 404, Message = "User not found" };
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> CreateUser(UserRequestDto user)
        {
            var newUser = new ApplicationUser();
            newUser.FirstName = user.FirstName;
            newUser.LastName = user.LastName;
            newUser.Email = user.Email;
            newUser.UserName = user.FirstName + user.LastName;



            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            return isCreated.Succeeded
                ? new ResponseDto { StatusCode = 201, Message = "User created successfully", Result = null }
                : new ResponseDto { StatusCode = 400, Message = "Error Occured while trying to create a user" };
        }

        /// <summary>
        /// Edits an existing user.
        /// </summary>
        /// <param name="user">Updated user details.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> EditUser(ApplicationUser user)
        {
            bool isUpdated = await _userRepository.EditUser(user);
            return isUpdated
                ? new ResponseDto { StatusCode = 200, Message = "User updated successfully", Result = user }
                : new ResponseDto { StatusCode = 404, Message = "User not found" };
        }

        /// <summary>
        /// Updates specific fields of a user.
        /// </summary>
        /// <param name="userUpdate">Updated user details.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> UpdateUser(ApplicationUser userUpdate)
        {
            bool isUpdated = await _userRepository.UpdateUser(userUpdate);
            return isUpdated
                ? new ResponseDto { StatusCode = 200, Message = "User updated successfully", Result = userUpdate }
                : new ResponseDto { StatusCode = 404, Message = "User not found" };
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> DeleteUser(int userId)
        {
            bool isDeleted = await _userRepository.DeleteUser(userId);
            return isDeleted
                ? new ResponseDto { StatusCode = 200, Message = "User deleted successfully" }
                : new ResponseDto { StatusCode = 404, Message = "User not found" };
        }
    }



}
