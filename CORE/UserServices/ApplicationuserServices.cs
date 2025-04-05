using DATA.DTO;
using DATA.Interface;
using DATA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Claim = System.Security.Claims.Claim;

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

        public IConfiguration _configuration { get; }



        /// <summary>
        /// Constructor to inject the user repository.
        /// </summary>
        /// <param name="userRepository">Instance of IUserRepository.</param>
        public ApplicationuserServices(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUserRepository userRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
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

        public async Task<ResponseDto> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users != null
                ? new ResponseDto { StatusCode = 200, Message = "Users found", Result = users }
                : new ResponseDto { StatusCode = 404, Message = "Users not found" };
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">User details.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> CreateUser(UserRequestDto user)
        {
            // Check if the email already exists
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if(existingUser != null)
            {
                return new ResponseDto { StatusCode = 400, Message = "Email already in use" };
            }

            // Create a new user
            var newUser = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.FirstName + user.LastName // You may want to refine this to avoid potential duplicates
            };

            // Create the user
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if(!isCreated.Succeeded)
            {
                // Return error with detailed messages from IdentityResult
                var errorMessage = string.Join(", ", isCreated.Errors.Select(e => e.Description));
                return new ResponseDto { StatusCode = 400, Message = $"Error occurred while trying to create a user: {errorMessage}" };
            }

            // Assign role to the user
            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "User");
            if(!addToRoleResult.Succeeded)
            {
                // If role assignment fails, delete the user and return the error
                await _userManager.DeleteAsync(newUser);
                var roleErrorMessage = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                return new ResponseDto { StatusCode = 400, Message = $"Error assigning role: {roleErrorMessage}" };
            }

            // Successfully created the user and assigned the role
            return new ResponseDto { StatusCode = 201, Message = "User created successfully", Result = null };
        }

        public async Task<LoginResponseDto> UserLogin(LoginRequestDto user)
        {
            var userLogin = await _userManager.FindByEmailAsync(user.Email);
            if(user == null)
            {
                return new LoginResponseDto { StatusCode = 400, Message = "Invalid username or password." };
            }



            if(await _userManager.CheckPasswordAsync(userLogin, user.Password))
            {
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
                var userdetails = new UserRsponseDto
                {
                    FirstName = userLogin.FirstName,
                    LastName = userLogin.LastName,
                    Email = userLogin.Email,
                };

                var jwtToken = GetToken(authClaims);

                return new LoginResponseDto
                {
                    StatusCode = 200,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    UserRsponse = userdetails
                };
            }

            return new LoginResponseDto { StatusCode = 401, Message = "Invalid username or password." };
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddDays(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;

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
